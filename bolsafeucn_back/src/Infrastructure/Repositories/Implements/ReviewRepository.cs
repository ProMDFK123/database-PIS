using bolsafeucn_back.src.Domain.Models;
using bolsafeucn_back.src.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using bolsafeucn_back.src.Infrastructure.Data; // 🔹 Importante: agrega esto para reconocer AppDbContext

namespace bolsafeucn_back.src.Infrastructure.Repositories.Implements
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Review>> GetByProviderIdAsync(string providerId)
        {
            return await _context.Reviews
                .Where(r => r.ProviderId == providerId)
                .Include(r => r.Student)
                .ToListAsync();
        }

        public async Task<double> GetAverageRatingAsync(string providerId)
        {
            return await _context.Reviews
                .Where(r => r.ProviderId == providerId)
                .AverageAsync(r => r.Rating);
        }
    }
}
