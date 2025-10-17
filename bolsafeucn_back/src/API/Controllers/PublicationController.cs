using System.Security.Claims;
using bolsafeucn_back.src.Application.DTOs.BaseResponse;
using bolsafeucn_back.src.Application.DTOs.JobAplicationDTO;
using bolsafeucn_back.src.Application.DTOs.PublicationDTO;
using bolsafeucn_back.src.Application.Services.Interfaces;
using bolsafeucn_back.src.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bolsafeucn_back.src.API.Controllers
{
    /// <summary>
    /// Controlador unificado para gestión de publicaciones (Ofertas laborales y Compra/Venta)
    /// </summary>
    [Route("api/publications")]
    [ApiController]
    public class PublicationController : ControllerBase
    {
        private readonly IPublicationService _publicationService;
        private readonly IUserRepository _userRepository;
        private readonly IOfferService _offerService;
        private readonly IBuySellService _buySellService;
        private readonly IJobApplicationService _jobApplicationService;
        private readonly ILogger<PublicationController> _logger;

        public PublicationController(
            IPublicationService publicationService,
            IUserRepository userRepository,
            IOfferService offerService,
            IBuySellService buySellService,
            IJobApplicationService jobApplicationService,
            ILogger<PublicationController> logger
        )
        {
            _publicationService = publicationService;
            _userRepository = userRepository;
            _offerService = offerService;
            _buySellService = buySellService;
            _jobApplicationService = jobApplicationService;
            _logger = logger;
        }

        #region Crear Publicaciones (Requiere autenticación)

        /// <summary>
        /// Crea una nueva oferta laboral o de voluntariado
        /// </summary>
        [HttpPost("offers")]
        [Authorize]
        public async Task<IActionResult> CreateOffer([FromBody] CreateOfferDTO dto)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null)
            {
                _logger.LogWarning("Usuario no autenticado intentando crear oferta");
                return Unauthorized(new GenericResponse<object>("Usuario no autenticado"));
            }

            if (!int.TryParse(userIdString, out var userId))
            {
                _logger.LogWarning("ID de usuario inválido: {UserId}", userIdString);
                return BadRequest(new GenericResponse<object>("ID de usuario inválido"));
            }

            var currentUser = await _userRepository.GetGeneralUserByIdAsync(userId);
            if (currentUser == null)
            {
                _logger.LogWarning("Usuario no encontrado: {UserId}", userId);
                return NotFound(new GenericResponse<object>("Usuario no encontrado"));
            }

            _logger.LogInformation("Usuario {UserId} creando oferta: {Title}", userId, dto.Title);
            var response = await _publicationService.CreateOfferAsync(dto, currentUser);

            if (response == null)
            {
                _logger.LogError("Error al crear oferta para usuario {UserId}", userId);
                return BadRequest(new GenericResponse<object>("Error al crear la oferta"));
            }

            return Ok(response);
        }

        /// <summary>
        /// Crea una nueva publicación de compra/venta
        /// </summary>
        [HttpPost("buysells")]
        [Authorize]
        public async Task<IActionResult> CreateBuySell([FromBody] CreateBuySellDTO dto)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null)
            {
                _logger.LogWarning(
                    "Usuario no autenticado intentando crear publicación de compra/venta"
                );
                return Unauthorized(new GenericResponse<object>("Usuario no autenticado"));
            }

            if (!int.TryParse(userIdString, out var userId))
            {
                _logger.LogWarning("ID de usuario inválido: {UserId}", userIdString);
                return BadRequest(new GenericResponse<object>("ID de usuario inválido"));
            }

            var currentUser = await _userRepository.GetGeneralUserByIdAsync(userId);
            if (currentUser == null)
            {
                _logger.LogWarning("Usuario no encontrado: {UserId}", userId);
                return NotFound(new GenericResponse<object>("Usuario no encontrado"));
            }

            _logger.LogInformation(
                "Usuario {UserId} creando publicación de compra/venta: {Title}",
                userId,
                dto.Title
            );
            var response = await _publicationService.CreateBuySellAsync(dto, currentUser);

            if (response == null)
            {
                _logger.LogError(
                    "Error al crear publicación de compra/venta para usuario {UserId}",
                    userId
                );
                return BadRequest(new GenericResponse<object>("Error al crear la publicación"));
            }

            return Ok(response);
        }

        #endregion

        #region Obtener Ofertas Laborales (Público)

        /// <summary>
        /// Obtiene todas las ofertas laborales activas
        /// </summary>
        [HttpGet("offers")]
        public async Task<IActionResult> GetActiveOffers()
        {
            _logger.LogInformation("GET /api/publications/offers - Obteniendo ofertas activas");
            var offers = await _offerService.GetActiveOffersAsync();
            return Ok(
                new GenericResponse<IEnumerable<object>>("Ofertas recuperadas exitosamente", offers)
            );
        }

        /// <summary>
        /// Obtiene los detalles de una oferta laboral específica
        /// </summary>
        [HttpGet("offers/{id}")]
        public async Task<IActionResult> GetOfferDetails(int id)
        {
            _logger.LogInformation(
                "GET /api/publications/offers/{Id} - Obteniendo detalles de oferta",
                id
            );
            var offer = await _offerService.GetOfferDetailsAsync(id);

            if (offer == null)
            {
                _logger.LogWarning("Oferta {Id} no encontrada", id);
                return NotFound(new GenericResponse<object>("Oferta no encontrada"));
            }

            return Ok(
                new GenericResponse<object>("Detalles de oferta recuperados exitosamente", offer)
            );
        }

        #endregion

        #region Obtener Publicaciones de Compra/Venta (Público)

        /// <summary>
        /// Obtiene todas las publicaciones de compra/venta activas
        /// </summary>
        [HttpGet("buysells")]
        public async Task<IActionResult> GetActiveBuySells()
        {
            _logger.LogInformation(
                "GET /api/publications/buysells - Obteniendo publicaciones de compra/venta activas"
            );
            var buySells = await _buySellService.GetActiveBuySellsAsync();
            return Ok(
                new GenericResponse<IEnumerable<object>>(
                    "Publicaciones de compra/venta recuperadas exitosamente",
                    buySells
                )
            );
        }

        /// <summary>
        /// Obtiene los detalles de una publicación de compra/venta específica
        /// </summary>
        [HttpGet("buysells/{id}")]
        public async Task<IActionResult> GetBuySellDetails(int id)
        {
            _logger.LogInformation(
                "GET /api/publications/buysells/{Id} - Obteniendo detalles de publicación",
                id
            );
            var buySell = await _buySellService.GetBuySellDetailsAsync(id);

            if (buySell == null)
            {
                _logger.LogWarning("Publicación de compra/venta {Id} no encontrada", id);
                return NotFound(new GenericResponse<object>("Publicación no encontrada"));
            }

            return Ok(
                new GenericResponse<object>(
                    "Detalles de publicación recuperados exitosamente",
                    buySell
                )
            );
        }

        #endregion

        #region Postulaciones a Ofertas (Requiere autenticación de estudiante)

        /// <summary>
        /// Permite a un estudiante postular a una oferta laboral
        /// SEGURIDAD: El studentId se obtiene del token JWT
        /// </summary>
        [HttpPost("offers/{id}/apply")]
        [Authorize]
        public async Task<ActionResult<JobApplicationResponseDto>> ApplyToOffer(
            int id,
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
                    "POST /api/publications/offers/{Id}/apply - Estudiante {StudentId} postulando a oferta",
                    id,
                    studentId
                );

                dto.JobOfferId = id;
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
        [HttpGet("offers/my-applications")]
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
                    "GET /api/publications/offers/my-applications - Obteniendo postulaciones del estudiante {StudentId}",
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
    }
}
