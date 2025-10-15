using bolsafeucn_back.src.Domain.Models;

namespace bolsafeucn_back.src.Infrastructure.Repositories.Interfaces
{
    public interface IVerificationCodeRepository
    {
        Task<VerificationCode> CreateCodeAsync(VerificationCode code);
        Task<VerificationCode> GetByLastUserIdAsync(int userId, CodeType tipo);
        Task<bool> DeleteByUserIdAsync(int userId, CodeType tipo);
    }
}
