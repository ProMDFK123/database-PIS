using bolsafeucn_back.src.Domain.Models;
using bolsafeucn_back.src.Infrastructure.Data;
using bolsafeucn_back.src.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace bolsafeucn_back.src.Infrastructure.Repositories.Implements;

public class OfferRepository : IOfferRepository
{
    private readonly AppDbContext _context;

    public OfferRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Offer>> GetAllActiveAsync()
    {
        return await _context.Offers
            .Include(o => o.Oferente)
            .ThenInclude(gu => gu.Company)
            .Include(o => o.Oferente)
            .ThenInclude(gu => gu.Individual)
            .Where(o => o.Activa && o.FechaFin > DateTime.UtcNow)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Offer?> GetByIdAsync(int offerId)
    {
        return await _context.Offers
            .Include(o => o.Oferente)
            .ThenInclude(gu => gu.Company)
            .Include(o => o.Oferente)
            .ThenInclude(gu => gu.Individual)
            .FirstOrDefaultAsync(o => o.Id == offerId);
    }
}
