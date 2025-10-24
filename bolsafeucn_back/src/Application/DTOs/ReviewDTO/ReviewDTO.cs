namespace bolsafeucn_back.src.Application.DTOs.ReviewDTO
{
    public class ReviewDTO
    {
        public float Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public string? StudentId { get; set; }
        public string? ProviderId { get; set; }
    }
}
