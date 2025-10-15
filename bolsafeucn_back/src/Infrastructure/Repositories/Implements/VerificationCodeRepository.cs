using bolsafeucn_back.src.Domain.Models;
using bolsafeucn_back.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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

        public async Task<VerificationCode> GetByLatestUserIdAsync(int userId, CodeType codeType)
        {
            var verificationCode = await _context
                .VerificationCodes.Where(vc =>
                    vc.GeneralUserId == userId && vc.CodeType == codeType
                )
                .OrderByDescending(vc => vc.CreatedAt)
                .FirstOrDefaultAsync();
            return verificationCode!;
        }

        public async Task<int> IncreaseAttemptsAsync(int userId, CodeType codeType)
        {
            var verificationCode = await _context
                .VerificationCodes.Where(vc =>
                    vc.GeneralUserId == userId && vc.CodeType == codeType
                )
                .OrderByDescending(vc => vc.CreatedAt)
                .ExecuteUpdateAsync(vc => vc.SetProperty(v => v.Attempts, v => v.Attempts + 1));
            return await _context
                .VerificationCodes.Where(vc =>
                    vc.GeneralUserId == userId && vc.CodeType == codeType
                )
                .OrderByDescending(vc => vc.CreatedAt)
                .Select(vc => vc.Attempts)
                .FirstAsync();
        }

        public async Task<bool> DeleteByUserIdAsync(int userId, CodeType codeType)
        {
            await _context
                .VerificationCodes.Where(vc =>
                    vc.GeneralUserId == userId && vc.CodeType == codeType
                )
                .ExecuteDeleteAsync();
            var exists = await _context.VerificationCodes.AnyAsync(vc =>
                vc.GeneralUserId == userId && vc.CodeType == codeType
            );
            return !exists;
        }
    }
}
