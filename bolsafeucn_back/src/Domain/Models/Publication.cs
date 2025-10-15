namespace bolsafeucn_back.src.Domain.Models
{
    public enum Types
    {
        Offer,
        Volunteer,
        BuySell,
    }

    public abstract class Publication
    {
        public int Id { get; set; }
        public required GeneralUser Oferente { get; set; }
        public required string UserId { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTime PublicationDate { get; set; } = DateTime.Now;
        public ICollection<Image> Images { get; set; } = new List<Image>();
        public required Types Type { get; set; }
        public bool IsActive { get; set; }
    }
}
