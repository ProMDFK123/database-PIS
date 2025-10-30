using bolsafeucn_back.src.Domain.Models;

namespace bolsafeucn_back.src.Application.DTOs.OfferDTOs
{
    /// <summary>
    /// Dto que se usa al momento en que el admin administra las publicaciones ya publicadas y se le muestran las ofertas 
    /// </summary>
    public class OfferBasicAdminDto
    {
        public string Title { get; set; }
        public required string CompanyName { get; set; }
        public DateTime PublicationDate { get; set; }
        public OfferTypes OfferType { get; set; }
        public bool Activa { get; set; }  
    }
}