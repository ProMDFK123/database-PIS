using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bolsafeucn_back.src.Application.Services.Interfaces;
using bolsafeucn_back.src.Application.DTOs;
using bolsafeucn_back.src.Domain.Models;

using bolsafeucn_back.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace bolsafeucn_back.src.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OffersController : ControllerBase
    {
        private readonly IOfferService _offerService;

        public OffersController(IOfferService offerService)
        {
            _offerService = offerService;
        }

        // GET: api/Offers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OfferDetailsDto>> GetOffer(int id)
        {
            var offerDetails = await _offerService.GetOfferDetailsAsync(id);
            if (offerDetails == null)
            {
                return NotFound(new { message = "Oferta no encontrada" });
            }
            return Ok(new { message = "Detalles de la oferta recuperados con éxito", data = offerDetails });
        }
    }
}