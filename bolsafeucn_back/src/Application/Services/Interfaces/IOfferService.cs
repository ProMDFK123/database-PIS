using bolsafeucn_back.src.Application.DTOs.OfferDTOs;

namespace bolsafeucn_back.src.Application.Services.Interfaces;

public interface IOfferService
{
    Task<OfferDetailDto?> GetOfferDetailsAsync(int offerId);
    Task<IEnumerable<OfferSummaryDto>> GetActiveOffersAsync();
    Task PublishOfferAsync(int id);
    Task RejectOfferAsync(int id);
    Task<IEnumerable<OfferSummaryDto>> GetPendingOffersAsync();
    Task<IEnumerable<OfferSummaryDto>> GetPublishedOffersAsync();
}
