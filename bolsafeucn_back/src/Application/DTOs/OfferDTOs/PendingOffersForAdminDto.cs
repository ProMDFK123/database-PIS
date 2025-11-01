using bolsafeucn_back.src.Domain.Models;

namespace bolsafeucn_back.src.Application.DTOs.OfferDTOs
{
    /// <summary>
    /// Dto para obtener info muy basica para pagina de validar ofertas para admin
    /// </summary>
    public class PendingOffersForAdminDto
    {
        public required string Title { get; set; }
        public Types Type { get; set; }
    }
}