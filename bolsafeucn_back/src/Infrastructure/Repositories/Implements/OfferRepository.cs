using bolsafeucn_back.src.Domain.Models;
using bolsafeucn_back.src.Infrastructure.Data;
using bolsafeucn_back.src.Infrastructure.Repositories.Interfaces;

namespace bolsafeucn_back.src.Infrastructure.Repositories.Implements
{
    public class OfferRepository : IOfferRepository
    {
        private readonly AppDbContext _context;

        public OfferRepository(AppDbContext context)
        {
            _context = context;
        }

        async Task<Offer> IOfferRepository.CreateOfferAsync(Offer offer)
        {
            try
            {
                _context.Publications.Add(offer);
                await _context.SaveChangesAsync();
                return offer;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear la oferta", ex);
            }
        }

        Task<Offer?> IOfferRepository.GetOfferByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Offer>> IOfferRepository.GetAllOffersAsync()
        {
            throw new NotImplementedException();
        }

        Task<bool> IOfferRepository.UpdateOfferAsync(Offer offer)
        {
            throw new NotImplementedException();
        }

        Task<bool> IOfferRepository.DeleteOfferAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
