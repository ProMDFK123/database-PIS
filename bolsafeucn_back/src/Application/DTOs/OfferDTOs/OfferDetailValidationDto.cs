using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bolsafeucn_back.src.Application.DTOs.OfferDTOs
{
    /// <summary>
    /// Dto para obtener toda la info necesaria de una oferta para que el admin pueda validarla
    /// </summary>
    /// TODO: agregar descripcion de compañia
    public class OfferDetailValidationDto
    {
        public string Title { get; set; } 
        public ICollection<string> Images { get; set; }
        public string Description { get; set; }
        public string CompanyName { get; set; }
        public string CorreoContacto { get; set; }
        public string TelefonoContacto { get; set; } 

    }
}