using bolsafeucn_back.src.Domain.Models;
using bolsafeucn_back.src.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

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
            Log.Information(
                "Creando código de verificación para usuario ID: {UserId}, Tipo: {CodeType}",
                code.GeneralUserId,
                code.CodeType
            );
            await _context.VerificationCodes.AddAsync(code);
            await _context.SaveChangesAsync();
            Log.Information(
                "Código de verificación creado exitosamente para usuario ID: {UserId}, Código ID: {CodeId}",
                code.GeneralUserId,
                code.Id
            );
            return code;
        }

        public async Task<VerificationCode> UpdateCodeAsync(VerificationCode code)
        {
            Log.Information(
                "Actualizando código de verificación ID: {CodeId} para usuario ID: {UserId}",
                code.Id,
                code.GeneralUserId
            );
            await _context
                .VerificationCodes.Where(vc => vc.Id == code.Id)
                .ExecuteUpdateAsync(vc =>
                    vc.SetProperty(v => v.Code, code.Code)
                        .SetProperty(v => v.Attempts, code.Attempts)
                        .SetProperty(v => v.Expiration, code.Expiration)
                );
            await _context.SaveChangesAsync();
            Log.Information(
                "Código de verificación ID: {CodeId} actualizado exitosamente",
                code.Id
            );
            var newVerificationCode = await _context
                .VerificationCodes.AsNoTracking()
                .FirstOrDefaultAsync(vc => vc.Id == code.Id);
            return newVerificationCode!;
        }

        public async Task<VerificationCode> GetByLatestUserIdAsync(int userId, CodeType codeType)
        {
            Log.Information(
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
                Log.Information(
                    "Código de verificación encontrado para usuario ID: {UserId}, Código ID: {CodeId}",
                    userId,
                    verificationCode.Id
                );
            }
            else
            {
                Log.Warning(
                    "No se encontró código de verificación para usuario ID: {UserId}, Tipo: {CodeType}",
                    userId,
                    codeType
                );
            }
            return verificationCode!;
        }

        public async Task<int> IncreaseAttemptsAsync(int userId, CodeType codeType)
        {
            Log.Information(
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
            Log.Information(
                "Intentos incrementados para usuario ID: {UserId}, Total intentos: {Attempts}",
                userId,
                attempts
            );
            return attempts;
        }

        public async Task<bool> DeleteByUserIdAsync(int userId, CodeType codeType)
        {
            Log.Information(
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
                Log.Information(
                    "Códigos de verificación eliminados exitosamente para usuario ID: {UserId}",
                    userId
                );
            }
            else
            {
                Log.Warning(
                    "Error al eliminar códigos de verificación para usuario ID: {UserId}",
                    userId
                );
            }
            return !exists;
        }
    }
}
