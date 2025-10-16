using bolsafeucn_back.src.Domain.Models;

namespace bolsafeucn_back.src.Infrastructure.Repositories.Interfaces;

public interface IOfferRepository
{
    Task<Offer?> GetByIdAsync(int offerId);
    Task<IEnumerable<Offer>> GetAllActiveAsync();
}
