using bolsafeucn_back.src.Domain.Models;

namespace bolsafeucn_back.src.Infrastructure.Repositories.Interfaces
{
    public interface IOfferRepository
    {
        // Define methods for Offer repository here
        Task<Offer> CreateOfferAsync(Offer offer);
        Task<Offer?> GetOfferByIdAsync(int id);
        Task<IEnumerable<Offer>> GetAllOffersAsync();
        Task<bool> UpdateOfferAsync(Offer offer);
        Task<bool> DeleteOfferAsync(int id);
    }
}
