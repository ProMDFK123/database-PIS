using bolsafeucn_back.src.Domain.Models;
using bolsafeucn_back.src.Infrastructure.Data;
using bolsafeucn_back.src.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace bolsafeucn_back.src.Infrastructure.Repositories.Implements;

public class OfferRepository : IOfferRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<OfferRepository> _logger;

    public OfferRepository(AppDbContext context, ILogger<OfferRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Offer>> GetAllActiveAsync()
    {
        _logger.LogInformation("Consultando ofertas activas en la base de datos");
        var offers = await _context
            .Offers.Include(o => o.Oferente)
            .ThenInclude(gu => gu.Company)
            .Include(o => o.Oferente)
            .ThenInclude(gu => gu.Individual)
            .Where(o => o.Activa && o.FechaFin > DateTime.UtcNow)
            .AsNoTracking()
            .ToListAsync();
        _logger.LogInformation(
            "Consulta completada: {Count} ofertas activas encontradas",
            offers.Count
        );
        return offers;
    }

    public async Task<Offer?> GetByIdAsync(int offerId)
    {
        _logger.LogInformation("Consultando oferta ID: {OfferId} en la base de datos", offerId);
        var offer = await _context
            .Offers.Include(o => o.Oferente)
            .ThenInclude(gu => gu.Company)
            .Include(o => o.Oferente)
            .ThenInclude(gu => gu.Individual)
            .FirstOrDefaultAsync(o => o.Id == offerId);

        if (offer != null)
        {
            _logger.LogInformation("Oferta ID: {OfferId} encontrada en la base de datos", offerId);
        }
        else
        {
            _logger.LogWarning("Oferta ID: {OfferId} no encontrada en la base de datos", offerId);
        }
        return offer;
    }
}
