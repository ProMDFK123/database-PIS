using bolsafeucn_back.src.Domain.Models;

namespace bolsafeucn_back.src.Infrastructure.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<GeneralUser>> GetAllAsync();
        Task<GeneralUser?> GetByIdAsync(int id);
        Task<GeneralUser> AddAsync(GeneralUser usuario);
        Task<bool> DeleteAsync(int id);
    }
}
