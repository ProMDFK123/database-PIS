namespace bolsafeucn_back.src.Application.DTOs.OfferDTOs
{
    public class OfferValidationDto
    {
        // true = aceptar la oferta, false = rechazarla
        public required string Accepted { get; set; }
    }
}