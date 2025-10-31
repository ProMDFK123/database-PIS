using System.Security.Claims;
using bolsafeucn_back.src.Application.DTOs.BaseResponse;
using bolsafeucn_back.src.Application.DTOs.JobAplicationDTO;
using bolsafeucn_back.src.Application.DTOs.OfferDTOs;
using bolsafeucn_back.src.Application.DTOs.PublicationDTO;
using bolsafeucn_back.src.Application.Services.Implements;
using bolsafeucn_back.src.Application.Services.Interfaces;
using bolsafeucn_back.src.Domain.Models;
using bolsafeucn_back.src.Infrastructure.Repositories.Interfaces;
using Mapster;
using MapsterMapper;
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
        private readonly IPublicationRepository _publicationRepository;

        public PublicationController(
            IPublicationService publicationService,
            IUserRepository userRepository,
            IOfferService offerService,
            IBuySellService buySellService,
            IJobApplicationService jobApplicationService,
            ILogger<PublicationController> logger,
            IPublicationRepository publicationRepository
        )
        {
            _publicationService = publicationService;
            _userRepository = userRepository;
            _offerService = offerService;
            _buySellService = buySellService;
            _jobApplicationService = jobApplicationService;
            _logger = logger;
            _publicationRepository = publicationRepository;
        }

        #region Crear Publicaciones (Requiere autenticación)

        /// <summary>
        /// Crea una nueva oferta laboral o de voluntariado
        /// Cualquier usuario autenticado puede crear ofertas
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
        /// Cualquier usuario autenticado puede crear publicaciones de compra/venta
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

        #region Obtiene publicaciones y mas (admin)

        /// <summary>
        /// Obtiene todas las ofertas pendientes de validación solo disponibles para admin
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("InProcess-offers")]
        public async Task<IActionResult> GetPendingOffersForAdmin()
        {
            var offer = await _offerService.GetPendingOffersAsync();
            if (offer == null)
            {
                return NotFound(new GenericResponse<string>("No hay ofertas pendientes", null));
            }
            return Ok(
                new GenericResponse<IEnumerable<OfferSummaryDto>>(
                    "Ofertas pendientes obtenidas",
                    offer
                )
            );
        }

        /// <summary>
        /// Obtiene todas las publicaciones de compra/venta pendientes de validación solo disponibles para admin
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("InProcess-buysells")]
        public async Task<IActionResult> GetPendingBuySellsForAdmin()
        {
            var buySell = await _buySellService.GetAllPendingBuySellsAsync();
            if (buySell == null)
            {
                return NotFound(new GenericResponse<string>("No hay publicaciones de compra/venta pendientes", null));
            }
            return Ok(new GenericResponse<IEnumerable<BuySellSummaryDto>>("Publicaciones de compra/venta pendientes obtenidas", buySell));
        }

        /// <summary>
        /// Obtiene todas las ofertas publicadas solo disponibles para admin
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("published-offers")]
        public async Task<IActionResult> GetPublishedOffersForAdmin()
        {
            var offer = await _offerService.GetPublishedOffersAsync();
            if (offer == null)
            {
                return NotFound(new GenericResponse<string>("No hay ofertas publicadas", null));
            }
            return Ok(
                new GenericResponse<IEnumerable<OfferBasicAdminDto>>(
                    "Ofertas publicadas obtenidas",
                    offer
                )
            );
        }

        /// <summary>
        /// Obtiene todas las compra y venta publicadas solo disponibles para admin
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("published-buysells")]
        public async Task<IActionResult> GetPublishedBuysellsForAdmin()
        {
            var buysell = await _buySellService.GetPublishedBuysellsAsync();
            if (buysell == null)
            {
                return NotFound(new GenericResponse<string>("no hay compra y ventas publicadas", null));
            }
            return Ok(
                new GenericResponse<IEnumerable<BuySellBasicAdminDto>>(
                    "Compra y ventas publicadas obtenidas",
                    buysell
                )
            );
        }

        #endregion

        #region Administrar ofertas y compra/venta (admin)

        /// <summary>
        /// Obtiene los detalles de una oferta para la administracion de esta
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("offers-admin/{offerId}")]
        public async Task<IActionResult> GetOfferDetailsForAdmin(int offerId)
        {
            var offer = await _offerService.GetOfferDetailsForAdminManagement(offerId);
            if (offer == null)
            {
                return NotFound(new GenericResponse<object>("No se encontro la oferta", null));
            }
            return Ok(new GenericResponse<OfferDetailsAdminDto>("Informacion basica de oferta recibida con exito.", offer));
        }

        /// <summary>
        /// Obtiene una lista de todos los postulantes inscritos a una oferta de trabajo
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("view-applicants-admin/{offerId}")]
        public async Task<IActionResult> GetApplicantsForAdmin(int offerId)
        {
            var applicants = await _jobApplicationService.GetApplicantsForAdminManagement(offerId);
            if (applicants == null)
            {
                return NotFound(new GenericResponse<object>("No se encontro la oferta", null));
            }
            return Ok(new GenericResponse<IEnumerable<ViewApplicantsDto>>("Lista de postulantes recibida exitosamente.", applicants));
        }

        /// <summary>
        /// Obtiene los detalles de un postulante inscrito a una oferta de trabajo
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("view-applicants-details-admin/{studentId}")]
        public async Task<IActionResult> ViewApplicantDetailsForAdmin(int studentId)
        {
            var applicantDetail = await _jobApplicationService.GetApplicantDetailForAdmin(studentId);
            if (applicantDetail == null)
            {
                return NotFound(new GenericResponse<object>("No se encontro al postulante", null));
            }
            return Ok(new GenericResponse<ViewApplicantDetailAdminDto>("Informacion basica de oferta recibida con exito.", applicantDetail));
        }

        #endregion

        #region Validar ofertas (admin)

        /// <summary>
        /// Valida o rechaza una oferta laboral específica (solo admin)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPost("validate/{id}")]
        public async Task<IActionResult> OfferValidation(
            int id,
            [FromBody] OfferValidationDto offerValidationDto
        )
        {
            var offer = await _offerService.GetOfferDetailsAsync(id);
            if (offer == null)
                return NotFound("Offer doesn't exist.");

            if (
                offerValidationDto == null
                || string.IsNullOrWhiteSpace(offerValidationDto.Accepted)
            )
                return BadRequest("Field 'Accepted'  is required, use yes or no");

            var decision = offerValidationDto.Accepted.Trim().ToLower();

            if (decision != "yes" && decision != "no")
                return BadRequest("The 'Accepted' field must be either 'yes' or 'no'.");

            bool isAccepted = decision == "yes";

            if (isAccepted)
            {
                await _offerService.PublishOfferAsync(id);
                return Ok(new { message = $"Offer {id} has been published successfully." });
            }
            else
            {
                await _offerService.RejectOfferAsync(id);
                return Ok(new { message = $"Offer {id} was rejected." });
            }
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
        /// SEGURIDAD: Solo estudiantes ven información completa (contacto, requisitos)
        /// Otros usuarios ven información básica sin datos sensibles
        /// </summary>
        [HttpGet("offers/{id}")]
        public async Task<IActionResult> GetOfferDetails(int id)
        {
            _logger.LogInformation(
                "GET /api/publications/offers/{Id} - Obteniendo detalles de oferta",
                id
            );

            // Verificar si el usuario está autenticado y es estudiante
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            bool isStudent = false;
            string userTypeDebug = "NO AUTENTICADO";

            if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out int userId))
            {
                var currentUser = await _userRepository.GetGeneralUserByIdAsync(userId);
                if (currentUser != null)
                {
                    isStudent = currentUser.UserType == UserType.Estudiante;
                    userTypeDebug = currentUser.UserType.ToString();
                    _logger.LogInformation(
                        "Usuario autenticado: ID={UserId}, UserType={UserType}, EsEstudiante={IsStudent}",
                        userId,
                        userTypeDebug,
                        isStudent
                    );
                }
            }
            else
            {
                _logger.LogInformation("Usuario NO autenticado o sin token JWT válido");
            }

            var offer = await _offerService.GetOfferDetailsAsync(id);

            if (offer == null)
            {
                _logger.LogWarning("Oferta {Id} no encontrada", id);
                return NotFound(new GenericResponse<object>("Oferta no encontrada"));
            }

            // Si NO es estudiante, ocultar información sensible
            if (!isStudent)
            {
                var basicOffer = new
                {
                    Id = offer.Id,
                    Title = offer.Title,
                    CompanyName = offer.CompanyName,
                    Location = offer.Location,
                    PostDate = offer.PostDate,
                    EndDate = offer.EndDate,
                    OfferType = offer.OfferType,
                    // NO incluir: Description, Remuneration
                    Message = "⚠️ Debes ser estudiante y estar autenticado para ver descripción completa, requisitos y remuneración",
                    Debug_UserType = userTypeDebug,
                };

                _logger.LogInformation(
                    "Oferta {Id} - Usuario no-estudiante ({UserType}), devolviendo información básica SIN description/remuneration",
                    id,
                    userTypeDebug
                );

                return Ok(
                    new GenericResponse<object>(
                        "Información básica de oferta (inicia sesión como estudiante para ver detalles completos)",
                        basicOffer
                    )
                );
            }

            // Si es estudiante, devolver información completa
            _logger.LogInformation(
                "Oferta {Id} - Usuario estudiante, devolviendo información completa CON description/remuneration",
                id
            );

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
        /// POSTULACIÓN DIRECTA: No requiere body. Se valida CV obligatorio del perfil
        /// SEGURIDAD: Solo estudiantes pueden postular. El studentId se obtiene del token JWT
        /// </summary>
        [HttpPost("offers/{id}/apply")]
        [Authorize(Roles = "Applicant")]
        public async Task<ActionResult<JobApplicationResponseDto>> ApplyToOffer(int id)
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

                // Verificar que el usuario sea realmente un estudiante
                var currentUser = await _userRepository.GetGeneralUserByIdAsync(studentId);
                if (currentUser == null || currentUser.UserType != UserType.Estudiante)
                {
                    _logger.LogWarning(
                        "Usuario {UserId} con tipo {UserType} intentó postular (solo estudiantes permitidos)",
                        studentId,
                        currentUser?.UserType
                    );
                    return Forbid();
                }

                _logger.LogInformation(
                    "POST /api/publications/offers/{Id}/apply - Estudiante {StudentId} postulando a oferta",
                    id,
                    studentId
                );

                // Postulación directa - sin body
                var application = await _jobApplicationService.CreateApplicationAsync(
                    studentId,
                    id
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
                _logger.LogWarning(ex, "Postulación no autorizada - {Message}", ex.Message);
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
        /// SEGURIDAD: Solo estudiantes pueden ver sus postulaciones. El studentId se obtiene del token JWT
        /// </summary>
        [HttpGet("offers/my-applications")]
        [Authorize(Roles = "Applicant")]
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


        #region Obtener Publicaciones por Status(Filtro para Empresa y Particular)

        // Importante para usar el enum

        // ... (dentro de tu clase PublicationController)

        /// <summary>
        /// Obtiene todas las publicaciones PUBLICADAS del usuario autenticado.
        /// </summary>
        [HttpGet("my-published")]
        [Authorize]
        public async Task<IActionResult> GetMyPublishedPublications()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId == null)
                {
                    _logger.LogWarning("No cuenta con autorización");
                    return Unauthorized(
                        new GenericResponse<object>("No autenticado o token inválido")
                    );
                }

                var publicationsDto = await _publicationService.GetMyPublishedPublicationsAsync(
                    userId
                );

                return Ok(
                    new GenericResponse<IEnumerable<PublicationsDTO>>(
                        "Ofertas pendientes obtenidas",
                        publicationsDto
                    )
                );
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
        /// Obtiene todas las publicaciones PENDIENTES del usuario autenticado.
        /// </summary>
        [HttpGet("my-pending")]
        [Authorize]
        public async Task<IActionResult> GetMyPendingPublications()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId == null)
                {
                    _logger.LogWarning("No cuenta con autorización");
                    return Unauthorized(
                        new GenericResponse<object>("No autenticado o token inválido")
                    );
                }

                var publicationsDto = await _publicationService.GetMyPendingPublicationsAsync(
                    userId
                );

                return Ok(
                    new GenericResponse<IEnumerable<PublicationsDTO>>(
                        "Ofertas pendientes obtenidas",
                        publicationsDto
                    )
                );
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
        /// Obtiene todas las publicaciones RECHAZADAS del usuario autenticado.
        /// </summary>
        [HttpGet("my-rejected")]
        [Authorize]
        public async Task<IActionResult> GetMyRejectedPublications()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId == null)
                {
                    _logger.LogWarning("No cuenta con autorización");
                    return Unauthorized(
                        new GenericResponse<object>("No autenticado o token inválido")
                    );
                }

                var publicationsDto = await _publicationService.GetMyRejectedPublicationsAsync(
                    userId
                );

                return Ok(
                    new GenericResponse<IEnumerable<PublicationsDTO>>(
                        "Ofertas pendientes obtenidas",
                        publicationsDto
                    )
                );
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

        #endregion
    }
}
