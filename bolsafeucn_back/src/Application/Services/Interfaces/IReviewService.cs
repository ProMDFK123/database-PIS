using bolsafeucn_back.src.Application.DTOs;

namespace bolsafeucn_back.src.Application.Services.Interfaces
{
    public interface IReviewService
    {
        Task AddReviewAsync(ReviewDTO dto);
        Task<IEnumerable<ReviewDTO>> GetReviewsByProviderAsync(string providerId);
        Task<double> GetAverageRatingAsync(string providerId);
    }
}
