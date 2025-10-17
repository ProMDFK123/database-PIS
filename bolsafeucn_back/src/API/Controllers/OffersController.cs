using bolsafeucn_back.src.Application.DTOs;
using bolsafeucn_back.src.Application.DTOs.BaseResponse;
using bolsafeucn_back.src.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace bolsafeucn_back.src.API.Controllers;

[Route("api/offers")]
[ApiController]
public class OffersController : ControllerBase
{
    private readonly IOfferService _offerService;
    private readonly IJobApplicationService _jobApplicationService;
    private readonly ILogger<OffersController> _logger;

    public OffersController(
        IOfferService offerService,
        IJobApplicationService jobApplicationService,
        ILogger<OffersController> logger
    )
    {
        _offerService = offerService;
        _jobApplicationService = jobApplicationService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetActiveOffers()
    {
        _logger.LogInformation("Endpoint: GET /api/offers - Obteniendo lista de ofertas activas");
        var offers = await _offerService.GetActiveOffersAsync();
        return Ok(offers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOfferDetails(int id)
    {
        _logger.LogInformation(
            "Endpoint: GET /api/offers/{Id} - Obteniendo detalles de oferta",
            id
        );
        var offer = await _offerService.GetOfferDetailsAsync(id);
        return Ok(offer);
    }

    // POST: api/offers/{id}/apply
    [HttpPost("{id}/apply")]
    public async Task<ActionResult<JobApplicationResponseDto>> ApplyToOffer(
        int id,
        [FromBody] CreateJobApplicationDto dto,
        [FromQuery] int studentId
    )
    {
        try
        {
            _logger.LogInformation(
                "Endpoint: POST /api/offers/{Id}/apply - Estudiante {StudentId} postulando a oferta",
                id,
                studentId
            );
            dto.JobOfferId = id;
            var application = await _jobApplicationService.CreateApplicationAsync(studentId, dto);
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

    // GET: api/offers/my-applications
    [HttpGet("my-applications")]
    public async Task<ActionResult<IEnumerable<JobApplicationResponseDto>>> GetMyApplications(
        [FromQuery] int studentId
    )
    {
        _logger.LogInformation(
            "Endpoint: GET /api/offers/my-applications - Obteniendo postulaciones del estudiante {StudentId}",
            studentId
        );
        var applications = await _jobApplicationService.GetStudentApplicationsAsync(studentId);
        return Ok(
            new GenericResponse<IEnumerable<JobApplicationResponseDto>>(
                "Postulaciones recuperadas exitosamente",
                applications
            )
        );
    }
}
