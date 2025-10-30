using bolsafeucn_back.src.Application.DTOs.OfferDTOs;
using bolsafeucn_back.src.Application.Services.Interfaces;
using bolsafeucn_back.src.Domain.Models;
using bolsafeucn_back.src.Infrastructure.Data;
using bolsafeucn_back.src.Infrastructure.Repositories.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace bolsafeucn_back.src.Application.Services.Implements;

public class OfferService : IOfferService
{
    private readonly IOfferRepository _offerRepository;
    private readonly ILogger<OfferService> _logger;
    private readonly AppDbContext _context;

    public OfferService(
        IOfferRepository offerRepository,
        ILogger<OfferService> logger,
        AppDbContext context
    )
    {
        _offerRepository = offerRepository;
        _logger = logger;
        _context = context;
    }

    public async Task<IEnumerable<OfferSummaryDto>> GetActiveOffersAsync()
    {
        _logger.LogInformation("Obteniendo todas las ofertas activas");

        // Debe traer User + Company/Individual (el repo debería hacer Include).
        var offers = await _offerRepository.GetAllActiveAsync();
        var list = offers.ToList();

        _logger.LogInformation("Se encontraron {Count} ofertas activas", list.Count);

        var result = list.Select(o =>
            {
                // Nombre de oferente
                var ownerName =
                    o.User?.UserType == UserType.Empresa
                        ? (o.User.Company?.CompanyName ?? "Empresa desconocida")
                    : o.User?.UserType == UserType.Particular
                        ? $"{(o.User.Individual?.Name ?? "").Trim()} {(o.User.Individual?.LastName ?? "").Trim()}".Trim()
                    : (o.User?.UserName ?? "UCN");

                return new OfferSummaryDto
                {
                    Id = o.Id,
                    Title = o.Title,
                    CompanyName = ownerName, // si lo sigues usando en otros lados
                    OwnerName = ownerName, // lo que consume el front para “oferente”

                    Location = "Campus Antofagasta",

                    // 💰 y fechas para la tarjeta
                    Remuneration = o.Remuneration,
                    DeadlineDate = o.DeadlineDate,
                    PublicationDate = o.PublicationDate,
                    OfferType = o.OfferType, // Trabajo / Voluntariado (enum)
                };
            })
            .ToList();

        return result;
    }

    public async Task<OfferDetailDto?> GetOfferDetailsAsync(int offerId)
    {
        _logger.LogInformation("Obteniendo detalles de la oferta ID: {OfferId}", offerId);

        var offer = await _offerRepository.GetByIdAsync(offerId);
        if (offer == null)
        {
            _logger.LogWarning("Oferta con ID {OfferId} no encontrada", offerId);
            throw new KeyNotFoundException($"Offer with id {offerId} not found");
        }

        // Nombre de oferente para detalles
        var ownerName =
            offer.User?.UserType == UserType.Empresa
                ? (offer.User.Company?.CompanyName ?? "Empresa desconocida")
            : offer.User?.UserType == UserType.Particular
                ? $"{(offer.User.Individual?.Name ?? "").Trim()} {(offer.User.Individual?.LastName ?? "").Trim()}".Trim()
            : (offer.User?.UserName ?? "UCN");

        var result = new OfferDetailDto
        {
            Id = offer.Id,
            Title = offer.Title,
            Description = offer.Description,
            CompanyName = ownerName,
            // si también quieres forzar aquí Antofagasta:
            Location = "Campus Antofagasta",

            PostDate = offer.PublicationDate,
            EndDate = offer.EndDate,
            Remuneration = (int)offer.Remuneration, // tu DTO usa int
            OfferType = offer.OfferType.ToString(),
        };

        _logger.LogInformation("Detalles de oferta ID: {OfferId} obtenidos exitosamente", offerId);
        return result;
    }

    public async Task PublishOfferAsync(int id)
    {
        var offer = await _context.Offers.FindAsync(id);
        if (offer == null)
            throw new KeyNotFoundException("Offer not found.");

        offer.IsActive = true; // o Published / Active, según tu modelo
        _context.Offers.Update(offer);
        await _context.SaveChangesAsync();
    }

    public async Task RejectOfferAsync(int id)
    {
        var offer = await _context.Offers.FindAsync(id);
        if (offer == null)
            throw new KeyNotFoundException("Offer not found.");

        offer.IsActive = false;
        _context.Offers.Update(offer);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<OfferSummaryDto>> GetPendingOffersAsync()
    {
        var offer = await _offerRepository.GetAllPendingOffersAsync();
        var list = offer.ToList();
        var result = list.Select(o =>
            {
                var ownerName =
                    o.User?.UserType == UserType.Empresa
                        ? (o.User.Company?.CompanyName ?? "Empresa desconocida")
                    : o.User?.UserType == UserType.Particular
                        ? $"{(o.User.Individual?.Name ?? "").Trim()} {(o.User.Individual?.LastName ?? "").Trim()}".Trim()
                    : (o.User?.UserName ?? "UCN");
                return new OfferSummaryDto
                {
                    Id = o.Id,
                    Title = o.Title,
                    CompanyName = ownerName,
                    OwnerName = ownerName,
                    Location = "Campus Antofagasta",
                    Remuneration = o.Remuneration,
                    DeadlineDate = o.DeadlineDate,
                    PublicationDate = o.PublicationDate,
                    OfferType = o.OfferType,
                };
            })
            .ToList();
        return result;
    }

    public async Task<IEnumerable<OfferBasicAdminDto>> GetPublishedOffersAsync()
    {
        var offer = await _offerRepository.PublishedOffersAsync();
        var list = offer.ToList();
        var result = list.Select(o =>
            {
                var ownerName =
                    o.User?.UserType == UserType.Empresa
                        ? (o.User.Company?.CompanyName ?? "Empresa desconocida")
                    : o.User?.UserType == UserType.Particular
                        ? $"{(o.User.Individual?.Name ?? "").Trim()} {(o.User.Individual?.LastName ?? "").Trim()}".Trim()
                    : (o.User?.UserName ?? "UCN");
                return new OfferBasicAdminDto
                {
                    Title = o.Title,
                    CompanyName = ownerName,
                    PublicationDate = o.PublicationDate,
                    OfferType = o.OfferType,
                    Activa = o.IsActive
                };
            })
            .ToList();
        return result;
    }
}
