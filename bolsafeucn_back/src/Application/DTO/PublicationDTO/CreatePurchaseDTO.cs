namespace bolsafeucn_back.src.Application.DTO.PublicationDTO
{
    public class CreatePurchaseDTO
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Category { get; set; }
        public required decimal Price { get; set; }
        public required List<string> ImagesURL { get; set; } = new List<string>();
    }
}
