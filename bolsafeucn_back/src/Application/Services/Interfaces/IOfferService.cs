using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bolsafeucn_back.src.Application.DTOs;
using System.Threading.Tasks;
namespace bolsafeucn_back.src.Application.Services.Interfaces
{
    public interface IOfferService
    {
        Task<OfferDetailsDto?> GetOfferDetailsAsync(int id);
    }
}