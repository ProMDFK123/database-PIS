using bolsafeucn_back.src.Domain.Models;
using bolsafeucn_back.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace bolsafeucn_back.src.Infrastructure.Repositories.Interfaces
{
    public class VerificationCodeRepository : IVerificationCodeRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<VerificationCodeRepository> _logger;

        public VerificationCodeRepository(
            AppDbContext context,
            ILogger<VerificationCodeRepository> logger
        )
        {
            _context = context;
            _logger = logger;
        }

        public async Task<VerificationCode> CreateCodeAsync(VerificationCode code)
        {
            _logger.LogInformation(
                "Creando código de verificación para usuario ID: {UserId}, Tipo: {CodeType}",
                code.GeneralUserId,
                code.CodeType
            );
            await _context.VerificationCodes.AddAsync(code);
            await _context.SaveChangesAsync();
            _logger.LogInformation(
                "Código de verificación creado exitosamente para usuario ID: {UserId}, Código ID: {CodeId}",
                code.GeneralUserId,
                code.Id
            );
            return code;
        }

        public async Task<VerificationCode> GetByLatestUserIdAsync(int userId, CodeType codeType)
        {
            _logger.LogInformation(
                "Obteniendo último código de verificación para usuario ID: {UserId}, Tipo: {CodeType}",
                userId,
                codeType
            );
            var verificationCode = await _context
                .VerificationCodes.Where(vc =>
                    vc.GeneralUserId == userId && vc.CodeType == codeType
                )
                .OrderByDescending(vc => vc.CreatedAt)
                .FirstOrDefaultAsync();

            if (verificationCode != null)
            {
                _logger.LogInformation(
                    "Código de verificación encontrado para usuario ID: {UserId}, Código ID: {CodeId}",
                    userId,
                    verificationCode.Id
                );
            }
            else
            {
                _logger.LogWarning(
                    "No se encontró código de verificación para usuario ID: {UserId}, Tipo: {CodeType}",
                    userId,
                    codeType
                );
            }
            return verificationCode!;
        }

        public async Task<int> IncreaseAttemptsAsync(int userId, CodeType codeType)
        {
            _logger.LogInformation(
                "Incrementando intentos de verificación para usuario ID: {UserId}, Tipo: {CodeType}",
                userId,
                codeType
            );
            var verificationCode = await _context
                .VerificationCodes.Where(vc =>
                    vc.GeneralUserId == userId && vc.CodeType == codeType
                )
                .OrderByDescending(vc => vc.CreatedAt)
                .ExecuteUpdateAsync(vc => vc.SetProperty(v => v.Attempts, v => v.Attempts + 1));
            var attempts = await _context
                .VerificationCodes.Where(vc =>
                    vc.GeneralUserId == userId && vc.CodeType == codeType
                )
                .OrderByDescending(vc => vc.CreatedAt)
                .Select(vc => vc.Attempts)
                .FirstAsync();
            _logger.LogInformation(
                "Intentos incrementados para usuario ID: {UserId}, Total intentos: {Attempts}",
                userId,
                attempts
            );
            return attempts;
        }

        public async Task<bool> DeleteByUserIdAsync(int userId, CodeType codeType)
        {
            _logger.LogInformation(
                "Eliminando códigos de verificación para usuario ID: {UserId}, Tipo: {CodeType}",
                userId,
                codeType
            );
            await _context
                .VerificationCodes.Where(vc =>
                    vc.GeneralUserId == userId && vc.CodeType == codeType
                )
                .ExecuteDeleteAsync();
            var exists = await _context.VerificationCodes.AnyAsync(vc =>
                vc.GeneralUserId == userId && vc.CodeType == codeType
            );

            if (!exists)
            {
                _logger.LogInformation(
                    "Códigos de verificación eliminados exitosamente para usuario ID: {UserId}",
                    userId
                );
            }
            else
            {
                _logger.LogWarning(
                    "Error al eliminar códigos de verificación para usuario ID: {UserId}",
                    userId
                );
            }
            return !exists;
        }
    }
}
