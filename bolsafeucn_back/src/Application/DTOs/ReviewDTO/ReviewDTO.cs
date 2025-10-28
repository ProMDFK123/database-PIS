namespace bolsafeucn_back.src.Application.DTOs.ReviewDTO
{
    public class ReviewDTO
    {
        public int idReview { get; set; }        
        public int? RatingForStudent { get; set; }
        public string? CommentForStudent { get; set; }
        public int? RatingForOfferor { get; set; }
        public string? CommentForOfferor { get; set; }
        public bool AtTime { get; set; }
        public bool GoodPresentation { get; set; }
        public DateTime ReviewWindowEndDate { get; set; }
        public int IdUser { get; set; }
        public int IdUser2 { get; set; }
        public int IdPublication { get; set; }
    }
}
