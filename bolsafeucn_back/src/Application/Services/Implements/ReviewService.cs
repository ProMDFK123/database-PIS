using bolsafeucn_back.src.Application.DTOs;
using bolsafeucn_back.src.Application.Mappers;
using bolsafeucn_back.src.Application.Services.Interfaces;
using bolsafeucn_back.src.Infrastructure.Repositories.Interfaces;

namespace bolsafeucn_back.src.Application.Services.Implements
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _repository;

        public ReviewService(IReviewRepository repository)
        {
            _repository = repository;
        }

        public async Task AddReviewAsync(ReviewDTO dto)
        {
            var review = ReviewMapper.ToEntity(dto);
            await _repository.AddAsync(review);
        }

        public async Task<IEnumerable<ReviewDTO>> GetReviewsByProviderAsync(string providerId)
        {
            var reviews = await _repository.GetByProviderIdAsync(providerId);
            return reviews.Select(ReviewMapper.ToDTO);
        }

        public async Task<double> GetAverageRatingAsync(string providerId)
        {
            return await _repository.GetAverageRatingAsync(providerId);
        }
    }
}
