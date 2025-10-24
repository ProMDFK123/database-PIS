using bolsafeucn_back.src.Application.DTOs.ReviewDTO;
using bolsafeucn_back.src.Application.Mappers;
using bolsafeucn_back.src.Application.Services.Interfaces;
using bolsafeucn_back.src.Domain.Models;
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
            // var review = ReviewMapper.ToEntity(dto);
            // await _repository.AddAsync(review);
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ReviewDTO>> GetReviewsByOfferorAsync(int offerorId)
        {
            // TODO: Arreglar esto
            return null;
            // var reviews = await _repository.GetByOfferorIdAsync(offerorId);
            // return reviews.Select(ReviewMapper.ToDTO);
        }

        public async Task<double?> GetAverageRatingAsync(int offerorId)
        {
            return await _repository.GetAverageRatingAsync(offerorId);
        }

        public async Task AddStudentReviewAsync(ReviewForStudentDTO dto)
        {
            var review = await _repository.GetByPublicationIdAsync(dto.PublicationId);
            if(review == null)
                throw new KeyNotFoundException("No se ha encontrado una reseña para el ID de publicación dado.");
            ReviewMapper.studentUpdateReview(dto, review);
            if(review.OfferorReviewCompleted) {
                review.IsCompleted = true;
            }
        }

        public async Task AddOfferorReviewAsync(ReviewForOfferorDTO dto)
        {
            var review = await _repository.GetByPublicationIdAsync(dto.PublicationId);
            if(review == null)
                throw new KeyNotFoundException("No se ha encontrado una reseña para el ID de publicación dado.");
            ReviewMapper.offerorUpdateReview(dto, review);
            if(review.StudentReviewCompleted) {
                review.IsCompleted = true;
                //await BothReviewsCompletedAsync();
            }
        }

        public Task BothReviewsCompletedAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Review> CreateInitialReviewAsync(InitialReviewDTO dto)
        {
            var review = ReviewMapper.CreateInitialReviewAsync(dto);
            await _repository.AddAsync(review);
            return review;
        }
    }
}
