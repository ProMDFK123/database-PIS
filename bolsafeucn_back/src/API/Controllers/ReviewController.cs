using bolsafeucn_back.src.Application.DTOs.ReviewDTO;
using bolsafeucn_back.src.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace bolsafeucn_back.src.API.Controllers
{
    [ApiController]
    [Route("reviews/[action]")] // [action] es el nombre de la funcion.
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }


        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] ReviewDTO dto)
        {
            await _reviewService.AddReviewAsync(dto);
            return Ok("Review added successfully");
        }

        [HttpGet("{offerorId}")]
        public async Task<IActionResult> GetReviews(int offerorId)
        {
            var reviews = await _reviewService.GetReviewsByOfferorAsync(offerorId);
            return Ok(reviews);
        }

        [HttpGet("{offerorId}")]
        public async Task<IActionResult> GetAverage(int offerorId)
        {
            var avg = await _reviewService.GetAverageRatingAsync(offerorId);
            return Ok(avg);
        }

        [HttpPost("addStudentReview")]
        public async Task<IActionResult> AddStudentReview([FromBody] ReviewForStudentDTO dto)
        {
            await _reviewService.AddStudentReviewAsync(dto);
            return Ok("Student review added successfully");
        }
        [HttpPost("addOfferorReview")]
        public async Task<IActionResult> AddOfferorReview([FromBody] ReviewForOfferorDTO dto)
        {
            await _reviewService.AddOfferorReviewAsync(dto);
            return Ok("Offeror review added successfully");
        }
        [HttpPost]
        public async Task<IActionResult> AddInitialReview([FromBody] InitialReviewDTO dto)
        {
            await _reviewService.CreateInitialReviewAsync(dto);
            return Ok("Initial review added successfully");
        }
    }
}
