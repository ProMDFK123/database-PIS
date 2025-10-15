using bolsafeucn_back.src.Application.DTO.PublicationDTO;
using bolsafeucn_back.src.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace bolsafeucn_back.src.API.Controllers
{
    public class PublicationController(IPublicationService publicationService) : BaseController
    {
        private readonly IPublicationService _service = publicationService;

        [HttpPost("offers")]
        public async Task<IActionResult> CreateOffer([FromBody] CreateOfferDTO dto)
        {
            var response = await _service.CreateOfferAsync(dto);
            if (response == null)
                return BadRequest("Error creating offer");
            return Ok(response);
        }

        [HttpPost("purchases")]
        public async Task<IActionResult> CreatePurchase([FromBody] CreatePurchaseDTO dto)
        {
            var response = await _service.CreatePurchaseAsync(dto);
            if (response == null)
                return BadRequest("Error creating purchase");
            return Ok(response);
        }
    }
}
