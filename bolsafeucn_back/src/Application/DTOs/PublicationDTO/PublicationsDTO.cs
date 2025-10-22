using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bolsafeucn_back.src.Domain.Models;

namespace bolsafeucn_back.src.Application.DTOs.PublicationDTO
{
    public class PublicationsDTO
    {
        int IdPublication { get; set; }
        string Title { get; set; }
        Types types { get; set; }
        string Description { get; set; }
        DateTime PublicationDate { get; set; }
        ICollection<Image> Images { get; set; }
        bool IsActive { get; set; }
        StatusValidation statusValidation { get; set; }
    }
}
