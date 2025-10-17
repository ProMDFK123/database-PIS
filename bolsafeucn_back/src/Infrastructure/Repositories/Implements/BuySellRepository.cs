using bolsafeucn_back.src.Domain.Models;
using bolsafeucn_back.src.Infrastructure.Data;
using bolsafeucn_back.src.Infrastructure.Repositories.Interfaces;

namespace bolsafeucn_back.src.Infrastructure.Repositories.Implements
{
    public class BuySellRepository : IBuySellRepository
    {
        private readonly AppDbContext _context;

        public BuySellRepository(AppDbContext context)
        {
            _context = context;
        }

        async Task<BuySell> IBuySellRepository.CreateBuySellAsync(BuySell buySell)
        {
            try
            {
                _context.Publications.Add(buySell);
                await _context.SaveChangesAsync();
                return buySell;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear la publicación de compra/venta", ex);
            }
        }
    }
}
