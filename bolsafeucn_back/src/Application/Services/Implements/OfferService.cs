using bolsafeucn_back.src.Application.DTOs.OfferDTOs;
using bolsafeucn_back.src.Application.Services.Interfaces;
using bolsafeucn_back.src.Domain.Models;
using bolsafeucn_back.src.Infrastructure.Repositories.Interfaces;
using Mapster;

namespace bolsafeucn_back.src.Application.Services.Implements;

public class OfferService : IOfferService
{
    private readonly IOfferRepository _offerRepository;
    private readonly ILogger<OfferService> _logger;

    public OfferService(IOfferRepository offerRepository, ILogger<OfferService> logger)
    {
        _offerRepository = offerRepository;
        _logger = logger;
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
                CompanyName = ownerName,             // si lo sigues usando en otros lados
                OwnerName = ownerName,               // lo que consume el front para “oferente”
                
                
                Location = "Campus Antofagasta",

                // 💰 y fechas para la tarjeta
                Remuneration = o.Remuneration,
                DeadlineDate = o.DeadlineDate,
                PublicationDate = o.PublicationDate,
                OfferType = o.OfferType,            // Trabajo / Voluntariado (enum)
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
}
