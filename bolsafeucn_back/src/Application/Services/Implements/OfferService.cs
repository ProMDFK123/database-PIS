using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bolsafeucn_back.src.Application.DTOs;
using bolsafeucn_back.src.Application.Services.Interfaces;
using bolsafeucn_back.src.Domain.Models;
using bolsafeucn_back.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace bolsafeucn_back.src.Application.Services.Implements
{
    public class OfferService : IOfferService
    {
        private readonly AppDbContext _context;

        public OfferService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OfferDetailsDto?> GetOfferDetailsAsync(int id)
        {
            var offer = await _context.Offers.FirstOrDefaultAsync(o => o.Id == id);
            if (offer == null) return null;

            return new OfferDetailsDto
            {
                Id = offer.Id,
                Titulo = offer.Titulo,
                Descripcion = offer.Descripcion,
                Activa = offer.Activa
            };
        }
    }
}