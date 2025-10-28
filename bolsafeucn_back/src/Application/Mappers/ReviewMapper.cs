using bolsafeucn_back.src.Application.DTOs.ReviewDTO;
using bolsafeucn_back.src.Domain.Models;

namespace bolsafeucn_back.src.Application.Mappers
{
    public static class ReviewMapper
    {
        public static Review studentUpdateReview(ReviewForStudentDTO dto, Review review)
        {
            review.RatingForStudent = dto.RatingForStudent;
            review.CommentForStudent = dto.CommentForStudent;
            review.AtTime = dto.atTime;
            review.GoodPresentation = dto.goodPresentation;
            review.StudentReviewCompleted = true;
            return review;
        }
        public static Review offerorUpdateReview(ReviewForOfferorDTO dto, Review review)
        {
            review.RatingForOfferor = dto.RatingForOfferor;
            review.CommentForOfferor = dto.CommentForOfferor;
            review.OfferorReviewCompleted = true;
            return review;
        }
        public static Review CreateInitialReviewAsync(InitialReviewDTO dto)
        {
            return new Review
            {
                StudentId = dto.StudentId,
                OfferorId = dto.OfferorId,
                PublicationId = dto.PublicationId,
                ReviewWindowEndDate = DateTime.UtcNow.AddDays(14) // 14 días automáticos desde que se recibe el DTO
            };
        }
        // public static Review ToEntity(ReviewDTO dto)
        // {
        //     return new Review
        //     {
        //         Rating = dto.Rating,
        //         Comment = dto.Comment,
        //         StudentId = dto.StudentId,
        //         ProviderId = dto.ProviderId
        //     };
        // }

        public static ReviewDTO ToDTO(Review entity)
        {
            return new ReviewDTO
            {
                idReview = entity.Id,
                RatingForStudent = entity.RatingForStudent,
                CommentForStudent = entity.CommentForStudent,
                RatingForOfferor = entity.RatingForOfferor,
                CommentForOfferor =  entity.CommentForOfferor,
                AtTime = entity.AtTime,
                GoodPresentation = entity.GoodPresentation,
                ReviewWindowEndDate = entity.ReviewWindowEndDate,
                IdUser = entity.StudentId ?? 0, // TODO: Revisar bien esto.
                IdUser2 = entity.OfferorId ?? 0,
                IdPublication = entity.PublicationId
            };
        }
    }
}

