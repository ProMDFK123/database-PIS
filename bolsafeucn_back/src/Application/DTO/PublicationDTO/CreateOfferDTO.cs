namespace bolsafeucn_back.src.Application.DTO.PublicationDTO
{
    public class CreateOfferDTO
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Category { get; set; }
        public required DateTime PublicationDate { get; set; }
        public required DateTime ExpirationDate { get; set; }
        public required decimal Remuneration { get; set; }
        public required List<string> ImagesURL { get; set; } = new List<string>();
    }
}
