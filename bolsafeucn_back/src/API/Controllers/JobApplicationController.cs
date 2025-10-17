using System.Security.Claims;
using bolsafeucn_back.src.Application.DTOs.BaseResponse;
using bolsafeucn_back.src.Application.DTOs.JobAplicationDTO;
using bolsafeucn_back.src.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bolsafeucn_back.src.API.Controllers
{
    /// <summary>
    /// Controlador para gestionar postulaciones a ofertas laborales
    /// </summary>
    [Route("api/job-applications")]
    [ApiController]
    public class JobApplicationController : ControllerBase
    {
        private readonly IJobApplicationService _jobApplicationService;
        private readonly ILogger<JobApplicationController> _logger;

        public JobApplicationController(
            IJobApplicationService jobApplicationService,
            ILogger<JobApplicationController> logger
        )
        {
            _jobApplicationService = jobApplicationService;
            _logger = logger;
        }

        #region Endpoints para Estudiantes

        /// <summary>
        /// Permite a un estudiante postular a una oferta laboral
        /// SEGURIDAD: El studentId se obtiene del token JWT
        /// </summary>
        [HttpPost("apply/{offerId}")]
        [Authorize]
        public async Task<ActionResult<JobApplicationResponseDto>> ApplyToOffer(
            int offerId,
            [FromBody] CreateJobApplicationDto dto
        )
        {
            try
            {
                // Obtener el ID del usuario desde el token JWT
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (
                    string.IsNullOrEmpty(userIdClaim)
                    || !int.TryParse(userIdClaim, out int studentId)
                )
                {
                    _logger.LogWarning("Token JWT inválido o sin claim de NameIdentifier");
                    return Unauthorized(
                        new GenericResponse<object>("No autenticado o token inválido")
                    );
                }

                _logger.LogInformation(
                    "POST /api/job-applications/apply/{OfferId} - Estudiante {StudentId} postulando",
                    offerId,
                    studentId
                );

                dto.JobOfferId = offerId;
                var application = await _jobApplicationService.CreateApplicationAsync(
                    studentId,
                    dto
                );

                return Ok(
                    new GenericResponse<JobApplicationResponseDto>(
                        "Postulación creada exitosamente",
                        application
                    )
                );
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Postulación no autorizada");
                return BadRequest(new GenericResponse<object>(ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Recurso no encontrado");
                return NotFound(new GenericResponse<object>(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Operación inválida");
                return Conflict(new GenericResponse<object>(ex.Message));
            }
        }

        /// <summary>
        /// Obtiene todas las postulaciones del estudiante autenticado
        /// SEGURIDAD: El studentId se obtiene del token JWT
        /// </summary>
        [HttpGet("my-applications")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<JobApplicationResponseDto>>> GetMyApplications()
        {
            try
            {
                // Obtener el ID del usuario desde el token JWT
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (
                    string.IsNullOrEmpty(userIdClaim)
                    || !int.TryParse(userIdClaim, out int studentId)
                )
                {
                    _logger.LogWarning("Token JWT inválido o sin claim de NameIdentifier");
                    return Unauthorized(
                        new GenericResponse<object>("No autenticado o token inválido")
                    );
                }

                _logger.LogInformation(
                    "GET /api/job-applications/my-applications - Estudiante {StudentId}",
                    studentId
                );

                var applications = await _jobApplicationService.GetStudentApplicationsAsync(
                    studentId
                );

                return Ok(
                    new GenericResponse<IEnumerable<JobApplicationResponseDto>>(
                        "Postulaciones recuperadas exitosamente",
                        applications
                    )
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener postulaciones");
                return StatusCode(
                    500,
                    new GenericResponse<object>("Error al obtener las postulaciones")
                );
            }
        }

        #endregion

        #region Endpoints para Empresas

        /// <summary>
        /// Obtiene todas las postulaciones para una oferta específica
        /// SEGURIDAD: Solo el creador de la oferta puede ver las postulaciones
        /// </summary>
        [HttpGet("offer/{offerId}")]
        [Authorize]
        public async Task<
            ActionResult<IEnumerable<JobApplicationResponseDto>>
        > GetApplicationsByOffer(int offerId)
        {
            try
            {
                _logger.LogInformation(
                    "GET /api/job-applications/offer/{OfferId} - Obteniendo postulaciones",
                    offerId
                );

                var applications = await _jobApplicationService.GetApplicationsByOfferIdAsync(
                    offerId
                );

                return Ok(
                    new GenericResponse<IEnumerable<JobApplicationResponseDto>>(
                        $"Postulaciones de la oferta {offerId} recuperadas exitosamente",
                        applications
                    )
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error al obtener postulaciones de la oferta {OfferId}",
                    offerId
                );
                return StatusCode(
                    500,
                    new GenericResponse<object>("Error al obtener las postulaciones")
                );
            }
        }

        /// <summary>
        /// Obtiene todas las postulaciones de todas las ofertas de la empresa autenticada
        /// SEGURIDAD: El companyId se obtiene del token JWT
        /// </summary>
        [HttpGet("my-offers-applications")]
        [Authorize]
        public async Task<
            ActionResult<IEnumerable<JobApplicationResponseDto>>
        > GetMyOffersApplications()
        {
            try
            {
                // Obtener el ID del usuario desde el token JWT
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (
                    string.IsNullOrEmpty(userIdClaim)
                    || !int.TryParse(userIdClaim, out int companyId)
                )
                {
                    _logger.LogWarning("Token JWT inválido o sin claim de NameIdentifier");
                    return Unauthorized(
                        new GenericResponse<object>("No autenticado o token inválido")
                    );
                }

                _logger.LogInformation(
                    "GET /api/job-applications/my-offers-applications - Empresa {CompanyId}",
                    companyId
                );

                var applications = await _jobApplicationService.GetApplicationsByCompanyIdAsync(
                    companyId
                );

                return Ok(
                    new GenericResponse<IEnumerable<JobApplicationResponseDto>>(
                        "Postulaciones recibidas recuperadas exitosamente",
                        applications
                    )
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener postulaciones de la empresa");
                return StatusCode(
                    500,
                    new GenericResponse<object>("Error al obtener las postulaciones")
                );
            }
        }

        /// <summary>
        /// Actualiza el estado de una postulación (Pendiente, Aceptado, Rechazado)
        /// SEGURIDAD: Solo el creador de la oferta puede actualizar el estado
        /// </summary>
        [HttpPatch("{applicationId}/status")]
        [Authorize]
        public async Task<ActionResult> UpdateApplicationStatus(
            int applicationId,
            [FromBody] UpdateApplicationStatusDto dto
        )
        {
            try
            {
                // Obtener el ID del usuario desde el token JWT
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (
                    string.IsNullOrEmpty(userIdClaim)
                    || !int.TryParse(userIdClaim, out int companyId)
                )
                {
                    _logger.LogWarning("Token JWT inválido o sin claim de NameIdentifier");
                    return Unauthorized(
                        new GenericResponse<object>("No autenticado o token inválido")
                    );
                }

                _logger.LogInformation(
                    "PATCH /api/job-applications/{ApplicationId}/status - Empresa {CompanyId} actualizando a {NewStatus}",
                    applicationId,
                    companyId,
                    dto.NewStatus
                );

                var result = await _jobApplicationService.UpdateApplicationStatusAsync(
                    applicationId,
                    dto.NewStatus,
                    companyId
                );

                if (result)
                {
                    return Ok(
                        new GenericResponse<object>(
                            $"Estado de postulación actualizado a '{dto.NewStatus}' exitosamente",
                            null
                        )
                    );
                }

                return BadRequest(
                    new GenericResponse<object>("No se pudo actualizar el estado de la postulación")
                );
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Acceso no autorizado");
                return Unauthorized(new GenericResponse<object>(ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Recurso no encontrado");
                return NotFound(new GenericResponse<object>(ex.Message));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Argumento inválido");
                return BadRequest(new GenericResponse<object>(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar estado de postulación");
                return StatusCode(
                    500,
                    new GenericResponse<object>("Error al actualizar el estado de la postulación")
                );
            }
        }

        #endregion
    }

    /// <summary>
    /// DTO para actualizar el estado de una postulación
    /// </summary>
    public class UpdateApplicationStatusDto
    {
        public string NewStatus { get; set; } = null!;
    }
}
