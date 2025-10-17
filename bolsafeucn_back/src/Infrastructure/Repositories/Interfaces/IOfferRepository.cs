using bolsafeucn_back.src.Domain.Models;

namespace bolsafeucn_back.src.Infrastructure.Repositories.Interfaces
{
    public interface IOfferRepository
    {
        Task<Offer?> GetByIdAsync(int id);
        Task<IEnumerable<Offer>> GetAllActiveAsync();
        Task<Offer> AddAsync(Offer offer);
        Task<bool> UpdateAsync(Offer offer);
    }
}