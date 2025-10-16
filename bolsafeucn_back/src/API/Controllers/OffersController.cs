using bolsafeucn_back.src.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace bolsafeucn_back.src.API.Controllers;

[Route("api/offers")]
[ApiController]
public class OffersController : ControllerBase
{
    private readonly IOfferService _offerService;

    public OffersController(IOfferService offerService)
    {
        _offerService = offerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetActiveOffers()
    {
        var offers = await _offerService.GetActiveOffersAsync();
        return Ok(offers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOfferDetails(int id)
    {
        var offer = await _offerService.GetOfferDetailsAsync(id);
        return Ok(offer);
    }
}
