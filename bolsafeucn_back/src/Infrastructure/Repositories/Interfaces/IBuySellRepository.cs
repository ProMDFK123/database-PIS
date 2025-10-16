using bolsafeucn_back.src.Domain.Models;

namespace bolsafeucn_back.src.Infrastructure.Repositories.Interfaces
{
    public interface IBuySellRepository
    {
        Task<BuySell> CreateBuySellAsync(BuySell buySell);
    }
}
