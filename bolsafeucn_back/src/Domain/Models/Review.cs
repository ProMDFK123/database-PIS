namespace bolsafeucn_back.src.Domain.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int? RatingForStudent { get; set; }
        public string? CommentForStudent { get; set; }
        public int? RatingForProvider { get; set; }
        public string? CommentForProvider { get; set; }
        public bool AtTime = false;
        public bool GoodPresentation = false;
        public required DateTime ReviewWindowEndDate { get; set; }
        public GeneralUser? Student { get; set; }
        public int? StudentId { get; set; }
        public GeneralUser? Offeror { get; set; }
        public int? OfferorId { get; set; }
        public bool StudentReviewCompleted { get; set; } = false;
        public bool OfferorReviewCompleted { get; set; } = false;
        public bool IsCompleted { get; set; } = false;
        public Publication? Publication { get; set; }
        public required int PublicationId { get; set; }
    }
}

