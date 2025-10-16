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
        var offers = await _offerRepository.GetAllActiveAsync();
        return offers.Adapt<IEnumerable<OfferSummaryDto>>();
    }

    public async Task<OfferDetailDto?> GetOfferDetailsAsync(int offerId)
    {
        var offer = await _offerRepository.GetByIdAsync(offerId);
        if (offer == null)
        {
            _logger.LogWarning("Offer with id {offerId} not found", offerId);
            throw new KeyNotFoundException($"Offer with id {offerId} not found");
        }
        return offer.Adapt<OfferDetailDto>();
    }
}
