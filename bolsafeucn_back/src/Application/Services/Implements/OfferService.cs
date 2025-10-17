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
        var offers = await _offerRepository.GetAllActiveAsync();

        // Materializar la lista antes de mapear para evitar problemas con Include después de Select
        var offersList = offers.ToList();
        _logger.LogInformation("Se encontraron {Count} ofertas activas", offersList.Count);

        // Mapear manualmente para evitar conflictos con Mapster en queries complejas
        var result = offersList
            .Select(o => new OfferSummaryDto
            {
                Id = o.Id,
                Title = o.Title,
                CompanyName =
                    o.User?.UserType == UserType.Empresa
                        ? o.User.Company?.CompanyName ?? "Empresa Desconocida"
                    : o.User?.UserType == UserType.Particular
                        ? $"{o.User.Individual?.Name ?? ""} {o.User.Individual?.LastName ?? ""}".Trim()
                    : "Desconocido",
                Location = o.Location,
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

        _logger.LogInformation("Detalles de oferta ID: {OfferId} obtenidos exitosamente", offerId);

        // Mapear manualmente para evitar problemas con navegación
        var result = new OfferDetailDto
        {
            Id = offer.Id,
            Title = offer.Title,
            Description = offer.Description,
            CompanyName =
                offer.User?.UserType == UserType.Empresa
                    ? offer.User.Company?.CompanyName ?? "Empresa Desconocida"
                : offer.User?.UserType == UserType.Particular
                    ? $"{offer.User.Individual?.Name ?? ""} {offer.User.Individual?.LastName ?? ""}".Trim()
                : "Desconocido",
            Location = offer.Location,
            PostDate = offer.PublicationDate,
            EndDate = offer.EndDate,
            Remuneration = offer.Remuneration,
            OfferType = offer.OfferType.ToString(),
        };

        return result;
    }
}
