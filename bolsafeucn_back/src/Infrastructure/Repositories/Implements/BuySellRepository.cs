using bolsafeucn_back.src.Domain.Models;
using bolsafeucn_back.src.Infrastructure.Data;
using bolsafeucn_back.src.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace bolsafeucn_back.src.Infrastructure.Repositories.Implements
{
    /// <summary>
    /// Repositorio para gestionar publicaciones de compra/venta
    /// </summary>
    public class BuySellRepository : IBuySellRepository
    {
        private readonly AppDbContext _context;

        public BuySellRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BuySell> CreateBuySellAsync(BuySell buySell)
        {
            try
            {
                _context.BuySells.Add(buySell);
                await _context.SaveChangesAsync();
                return buySell;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear la publicación de compra/venta", ex);
            }
        }

        public async Task<IEnumerable<BuySell>> GetAllActiveAsync()
        {
            return await _context
                .BuySells.Include(bs => bs.User)
                .ThenInclude(u => u.Company)
                .Include(bs => bs.User)
                .ThenInclude(u => u.Individual)
                .Include(bs => bs.Images)
                .Where(bs => bs.IsActive)
                .OrderByDescending(bs => bs.PublicationDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<BuySell?> GetByIdAsync(int id)
        {
            return await _context
                .BuySells.Include(bs => bs.User)
                .Include(bs => bs.Images)
                .FirstOrDefaultAsync(bs => bs.Id == id);
        }

        public async Task<IEnumerable<BuySell>> GetByUserIdAsync(int userId)
        {
            return await _context
                .BuySells.Where(bs => bs.UserId == userId)
                .Include(bs => bs.Images)
                .OrderByDescending(bs => bs.PublicationDate)
                .ToListAsync();
        }

        public async Task<BuySell> UpdateAsync(BuySell buySell)
        {
            try
            {
                _context.BuySells.Update(buySell);
                await _context.SaveChangesAsync();
                return buySell;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar la publicación de compra/venta", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var buySell = await GetByIdAsync(id);
                if (buySell == null)
                    return false;

                buySell.IsActive = false;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar la publicación de compra/venta", ex);
            }
        }

        public async Task<IEnumerable<BuySell>> SearchByCategoryAsync(string category)
        {
            return await _context
                .BuySells.Where(bs =>
                    bs.IsActive && bs.Category.ToLower().Contains(category.ToLower())
                )
                .Include(bs => bs.User)
                .Include(bs => bs.Images)
                .OrderByDescending(bs => bs.PublicationDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<BuySell>> SearchByPriceRangeAsync(
            decimal minPrice,
            decimal maxPrice
        )
        {
            return await _context
                .BuySells.Where(bs => bs.IsActive && bs.Price >= minPrice && bs.Price <= maxPrice)
                .Include(bs => bs.User)
                .Include(bs => bs.Images)
                .OrderBy(bs => bs.Price)
                .ToListAsync();
        }
    }
}
