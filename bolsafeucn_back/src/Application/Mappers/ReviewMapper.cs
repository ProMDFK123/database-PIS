using bolsafeucn_back.src.Application.DTOs;
using bolsafeucn_back.src.Domain.Models;

namespace bolsafeucn_back.src.Application.Mappers
{
    public static class ReviewMapper
    {
        public static Review ToEntity(ReviewDTO dto)
        {
            return new Review
            {
                Rating = dto.Rating,
                Comment = dto.Comment,
                StudentId = dto.StudentId,
                ProviderId = dto.ProviderId
            };
        }

        public static ReviewDTO ToDTO(Review entity)
        {
            return new ReviewDTO
            {
                Rating = entity.Rating,
                Comment = entity.Comment,
                StudentId = entity.StudentId,
                ProviderId = entity.ProviderId
            };
        }
    }
}

