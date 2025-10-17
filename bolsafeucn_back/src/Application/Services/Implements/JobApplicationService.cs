using bolsafeucn_back.src.Application.DTOs;
using bolsafeucn_back.src.Application.Services.Interfaces;
using bolsafeucn_back.src.Domain.Models;
using bolsafeucn_back.src.Infrastructure.Repositories.Interfaces;

namespace bolsafeucn_back.src.Application.Services.Implements
{
    public class JobApplicationService : IJobApplicationService
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;
        private readonly IOfferRepository _offerRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public JobApplicationService(
            IJobApplicationRepository jobApplicationRepository,
            IOfferRepository offerRepository,
            IUsuarioRepository usuarioRepository)
        {
            _jobApplicationRepository = jobApplicationRepository;
            _offerRepository = offerRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<JobApplicationResponseDto> CreateApplicationAsync(string studentId, CreateJobApplicationDto dto)
        {
            // Validar elegibilidad del estudiante
            if (!await ValidateStudentEligibilityAsync(studentId))
            {
                throw new UnauthorizedAccessException("El estudiante no es elegible para postular");
            }

            // Verificar que la oferta existe y está activa
            var offer = await _offerRepository.GetByIdAsync(dto.OfertaLaboralId);
            if (offer == null || !offer.Activa)
            {
                throw new KeyNotFoundException("La oferta no existe o no está activa");
            }

            // Verificar que no haya postulado anteriormente
            var existingApplication = await _jobApplicationRepository.GetByStudentAndOfferAsync(studentId, dto.OfertaLaboralId);
            if (existingApplication != null)
            {
                throw new InvalidOperationException("Ya has postulado a esta oferta");
            }

            // Obtener datos del estudiante
            var student = await _usuarioRepository.GetByIdAsync(int.Parse(studentId));
            if (student == null || student.Estudiante == null)
            {
                throw new KeyNotFoundException("Estudiante no encontrado");
            }

            // Crear la postulación
            var jobApplication = new JobApplication
            {
                EstudianteId = studentId,
                Estudiante = student,
                OfertaLaboralId = dto.OfertaLaboralId,
                OfertaLaboral = offer,
                Estado = "Pendiente",
                FechaPostulacion = DateTime.UtcNow
            };

            var createdApplication = await _jobApplicationRepository.AddAsync(jobApplication);

            return new JobApplicationResponseDto
            {
                Id = createdApplication.Id,
                EstudianteNombre = $"{student.Estudiante.Nombre} {student.Estudiante.Apellido}",
                EstudianteCorreo = student.Correo,
                OfertaTitulo = offer.Titulo,
                Estado = createdApplication.Estado,
                FechaPostulacion = createdApplication.FechaPostulacion,
                CurriculumVitae = student.Estudiante.CurriculumVitae,
                CartaMotivacional = dto.CartaMotivacional ?? student.Estudiante.CartaMotivacional
            };
        }

        public async Task<IEnumerable<JobApplicationResponseDto>> GetStudentApplicationsAsync(string studentId)
        {
            var applications = await _jobApplicationRepository.GetByStudentIdAsync(studentId);
            
            return applications.Select(app => new JobApplicationResponseDto
            {
                Id = app.Id,
                EstudianteNombre = $"{app.Estudiante.Estudiante?.Nombre} {app.Estudiante.Estudiante?.Apellido}",
                EstudianteCorreo = app.Estudiante.Correo,
                OfertaTitulo = app.OfertaLaboral.Titulo,
                Estado = app.Estado,
                FechaPostulacion = app.FechaPostulacion,
                CurriculumVitae = app.Estudiante.Estudiante?.CurriculumVitae,
                CartaMotivacional = app.Estudiante.Estudiante?.CartaMotivacional
            });
        }

        public async Task<bool> ValidateStudentEligibilityAsync(string studentId)
        {
            var student = await _usuarioRepository.GetByIdAsync(int.Parse(studentId));
            
            if (student == null || student.TipoUsuario != UserType.Student)
                return false;

            // Verificar que tenga correo institucional
            if (!student.Correo.EndsWith("@alumnos.ucn.cl"))
                return false;

            // Verificar que no esté bloqueado
            if (student.Bloqueado)
                return false;

            // Verificar que tenga CV
            if (student.Estudiante == null || string.IsNullOrEmpty(student.Estudiante.CurriculumVitae))
                return false;

            return true;
        }
    }
}