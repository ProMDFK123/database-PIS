namespace bolsafeucn_back.src.Domain.Models
{
    public class Review
    {
        public int Id { get; set; }

        public required float Rating { get; set; }

        public required string Comment { get; set; } = string.Empty;

        public GeneralUser? Student { get; set; }

        public string? StudentId { get; set; }

        public GeneralUser? Provider { get; set; }

        public string? ProviderId { get; set; }
    }
}

