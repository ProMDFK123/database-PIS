using bolsafeucn_back.src.Domain.Models;
using bolsafeucn_back.src.Infrastructure.Data;

namespace bolsafeucn_back.src.Infrastructure.Repositories.Interfaces
{
    public class VerificationCodeRepository : IVerificationCodeRepository
    {
        private readonly AppDbContext _context;

        public VerificationCodeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<VerificationCode> CreateCodeAsync(VerificationCode code)
        {
            await _context.VerificationCodes.AddAsync(code);
            await _context.SaveChangesAsync();
            return code;
        }

        public Task<VerificationCode?> GetByLastUserIdAsync(int userId, CodeType tipo)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteByUserIdAsync(int userId, CodeType tipo)
        {
            throw new NotImplementedException();
        }
    }
}
