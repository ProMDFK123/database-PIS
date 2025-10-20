using System.Security.Claims;
using bolsafeucn_back.src.Application.DTO.PublicationDTO;
using bolsafeucn_back.src.Application.Services.Interfaces;
using bolsafeucn_back.src.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using bolsafeucn_back.src.Application.Services.Implements;
using bolsafeucn_back.src.Application.DTO.BaseResponse;
using bolsafeucn_back.src.Application.DTOs.OfferDTOs;

namespace bolsafeucn_back.src.API.Controllers
{
    
    public class PublicationController(
        IPublicationService publicationService,
        IUserRepository userRepository,
        IOfferService offerService
    ) : BaseController
    {
        private readonly IOfferService _OfferService = offerService;
        private readonly IPublicationService _publicationService = publicationService;
        private readonly IUserRepository _userRepository = userRepository;
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


        [Authorize(Roles = "Admin")]
        [HttpPost("validate/{id}")]
        public async Task<IActionResult> OfferValidation(int id, [FromBody] OfferValidationDto offerValidationDto)
        {
            var offer = await _OfferService.GetOfferDetailsAsync(id);
            if (offer == null)
                return NotFound("Offer doesn't exist.");

            if (offerValidationDto == null || string.IsNullOrWhiteSpace(offerValidationDto.Accepted))
                return BadRequest("Field 'Accepted'  is required, use yes or no");

            var decision = offerValidationDto.Accepted.Trim().ToLower();

            if (decision != "yes" && decision != "no")
                return BadRequest("The 'Accepted' field must be either 'yes' or 'no'.");

            bool isAccepted = decision == "yes";

            if (isAccepted)
            {
                await _OfferService.PublishOfferAsync(id);
                return Ok(new { message = $"Offer {id} has been published successfully." });
            }
            else
            {
                await _OfferService.RejectOfferAsync(id);
                return Ok(new { message = $"Offer {id} was rejected." });
            }
        }
    }
}
