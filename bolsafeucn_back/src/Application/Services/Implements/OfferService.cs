using bolsafeucn_back.src.Application.DTOs.OfferDTOs;
using bolsafeucn_back.src.Application.Services.Interfaces;
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
        var offersList = offers.ToList();
        _logger.LogInformation("Se encontraron {Count} ofertas activas", offersList.Count);
        return offersList.Adapt<IEnumerable<OfferSummaryDto>>();
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
        return offer.Adapt<OfferDetailDto>();
    }
}
