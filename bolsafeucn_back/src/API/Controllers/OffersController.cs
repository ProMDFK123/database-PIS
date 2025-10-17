using bolsafeucn_back.src.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace bolsafeucn_back.src.API.Controllers;

[Route("api/offers")]
[ApiController]
public class OffersController : ControllerBase
{
    private readonly IOfferService _offerService;
    private readonly ILogger<OffersController> _logger;

    public OffersController(IOfferService offerService, ILogger<OffersController> logger)
    {
        _offerService = offerService;
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
}
