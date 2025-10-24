using bolsafeucn_back.src.Application.DTOs.ReviewDTO;
using bolsafeucn_back.src.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace bolsafeucn_back.src.API.Controllers
{
    [ApiController]
    [Route("api/reviews/[action]")]
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

        [HttpGet("{providerId}")]
        public async Task<IActionResult> GetReviews(int providerId)
        {
            var reviews = await _reviewService.GetReviewsByProviderAsync(providerId);
            return Ok(reviews);
        }

        [HttpGet("{providerId}")]
        public async Task<IActionResult> GetAverage(int providerId)
        {
            var avg = await _reviewService.GetAverageRatingAsync(providerId);
            return Ok(avg);
        }

        [HttpPost("addStudentReview")]
        public async Task<IActionResult> AddStudentReview([FromBody] ReviewForStudentDTO dto)
        {
            await _reviewService.AddStudentReviewAsync(dto);
            return Ok("Student review added successfully");
        }
    }
}
