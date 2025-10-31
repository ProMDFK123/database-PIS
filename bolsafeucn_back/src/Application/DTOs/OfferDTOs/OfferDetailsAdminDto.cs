using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bolsafeucn_back.src.Domain.Models;

namespace bolsafeucn_back.src.Application.DTOs.OfferDTOs
{
    /// <summary>
    /// Dto para que el administrador vea la informacion de la oferta y decida si
    /// </summary>
    public class OfferDetailsAdminDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<string> Images { get; set; }
        public string CompanyName { get; set; }
        public DateTime PublicationDate { get; set; }
        public Types Type { get; set; }
        public bool Active { get; set; }  
        public StatusValidation statusValidation { get; set; }
    }
}