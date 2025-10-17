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
        private readonly IUserRepository _userRepository;

        public JobApplicationService(
            IJobApplicationRepository jobApplicationRepository,
            IOfferRepository offerRepository,
            IUserRepository userRepository
        )
        {
            _jobApplicationRepository = jobApplicationRepository;
            _offerRepository = offerRepository;
            _userRepository = userRepository;
        }

        public async Task<JobApplicationResponseDto> CreateApplicationAsync(
            int studentId,
            CreateJobApplicationDto dto
        )
        {
            // Validar elegibilidad del estudiante
            if (!await ValidateStudentEligibilityAsync(studentId))
            {
                throw new UnauthorizedAccessException("El estudiante no es elegible para postular");
            }

            // Verificar que la oferta existe y está activa
            var offer = await _offerRepository.GetByIdAsync(dto.JobOfferId);
            if (offer == null || !offer.IsActive)
            {
                throw new KeyNotFoundException("La oferta no existe o no está activa");
            }

            // Verificar que no haya postulado anteriormente
            var existingApplication = await _jobApplicationRepository.GetByStudentAndOfferAsync(
                studentId,
                dto.JobOfferId
            );
            if (existingApplication != null)
            {
                throw new InvalidOperationException("Ya has postulado a esta oferta");
            }

            // Obtener datos del estudiante
            var student = await _userRepository.GetByIdAsync(studentId);
            if (student == null || student.Student == null)
            {
                throw new KeyNotFoundException("Estudiante no encontrado");
            }

            // Crear la postulación
            var jobApplication = new JobApplication
            {
                StudentId = studentId,
                Student = student,
                JobOfferId = dto.JobOfferId,
                JobOffer = offer,
                Status = "Pendiente",
                ApplicationDate = DateTime.UtcNow,
            };

            var createdApplication = await _jobApplicationRepository.AddAsync(jobApplication);

            return new JobApplicationResponseDto
            {
                Id = createdApplication.Id,
                StudentName = $"{student.Student.Name} {student.Student.LastName}",
                StudentEmail = student.Email!,
                OfferTitle = offer.Title,
                Status = createdApplication.Status,
                ApplicationDate = createdApplication.ApplicationDate,
                CurriculumVitae = student.Student.CurriculumVitae,
                MotivationLetter = dto.MotivationLetter ?? student.Student.MotivationLetter,
            };
        }

        public async Task<IEnumerable<JobApplicationResponseDto>> GetStudentApplicationsAsync(
            int studentId
        )
        {
            var applications = await _jobApplicationRepository.GetByStudentIdAsync(studentId);

            return applications.Select(app => new JobApplicationResponseDto
            {
                Id = app.Id,
                StudentName = $"{app.Student.Student?.Name} {app.Student.Student?.LastName}",
                StudentEmail = app.Student.Email!,
                OfferTitle = app.JobOffer.Title,
                Status = app.Status,
                ApplicationDate = app.ApplicationDate,
                CurriculumVitae = app.Student.Student?.CurriculumVitae,
                MotivationLetter = app.Student.Student?.MotivationLetter,
            });
        }

        public async Task<bool> ValidateStudentEligibilityAsync(int studentId)
        {
            var student = await _userRepository.GetByIdAsync(studentId);

            if (student == null || student.UserType != UserType.Estudiante)
                return false;

            // Verificar que tenga correo institucional
            if (!student.Email!.EndsWith("@alumnos.ucn.cl"))
                return false;

            // Verificar que no esté bloqueado
            if (student.Banned)
                return false;

            // Verificar que tenga CV
            if (student.Student == null || string.IsNullOrEmpty(student.Student.CurriculumVitae))
                return false;

            return true;
        }
    }
}
