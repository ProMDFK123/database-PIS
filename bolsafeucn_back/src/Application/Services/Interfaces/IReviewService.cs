using bolsafeucn_back.src.Application.DTOs.ReviewDTO;
using bolsafeucn_back.src.Domain.Models;

namespace bolsafeucn_back.src.Application.Services.Interfaces
{
    public interface IReviewService
    {
        Task AddStudentReviewAsync(ReviewForStudentDTO dto);
        Task AddOfferorReviewAsync(ReviewForOfferorDTO dto);
        Task BothReviewsCompletedAsync();
        Task AddReviewAsync(ReviewDTO dto);
        Task<IEnumerable<ReviewDTO>> GetReviewsByProviderAsync(int providerId);
        Task<double?> GetAverageRatingAsync(int providerId);
        Task<Review> CreateInitialReviewAsync(InitialReviewDTO dto);
    }
}
