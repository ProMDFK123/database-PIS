using System.Security.Claims;
using bolsafeucn_back.src.Application.DTO.PublicationDTO;
using bolsafeucn_back.src.Application.Services.Interfaces;
using bolsafeucn_back.src.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace bolsafeucn_back.src.API.Controllers
{
    public class PublicationController(
        IPublicationService publicationService,
        IUserService userService,
        IUserRepository userRepository
    ) : BaseController
    {
        private readonly IPublicationService _publicationService = publicationService;
        private readonly IUserService _userService = userService;
        private readonly IUserRepository _userRepository;
        [HttpPost("offers")]
        public async Task<IActionResult> CreateOffer([FromBody] CreateOfferDTO dto)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null)
                return Unauthorized("User not authenticated");
            if (!int.TryParse(userIdString, out var userId))
                return BadRequest("Invalid user ID");
            var currentUser = await _userRepository.GetGeneralUserByIdAsync(userId);
            if (currentUser == null)
                return NotFound("User not found");

            var response = await _publicationService.CreateOfferAsync(dto, currentUser);
            if (response == null)
                return BadRequest("Error creating offer");
            return Ok(response);
        }

        [HttpPost("buysells")]
        public async Task<IActionResult> CreateBuySell([FromBody] CreateBuySellDTO dto)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null)
                return Unauthorized("User not authenticated");
            if (!int.TryParse(userIdString, out var userId))
                return BadRequest("Invalid user ID");
            var currentUser = await _userRepository.GetGeneralUserByIdAsync(userId);
            if (currentUser == null)
                return NotFound("User not found");

            var response = await _publicationService.CreateBuySellAsync(dto, currentUser);
            if (response == null)
                return BadRequest("Error creating buy/sell");
            return Ok(response);
        }
    }
}
