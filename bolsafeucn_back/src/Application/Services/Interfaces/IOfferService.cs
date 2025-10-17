using bolsafeucn_back.src.Application.DTOs.OfferDTOs;

namespace bolsafeucn_back.src.Application.Services.Interfaces;

public interface IOfferService
{
    Task<OfferDetailDto?> GetOfferDetailsAsync(int offerId);
    Task<IEnumerable<OfferSummaryDto>> GetActiveOffersAsync();
}
