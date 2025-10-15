namespace bolsafeucn_back.src.Domain.Models
{
    public class Offer : Publication
    {
        public decimal Remuneration { get; set; }
        public DateTime OfferDate { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
