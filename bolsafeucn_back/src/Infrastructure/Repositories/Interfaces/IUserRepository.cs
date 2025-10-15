using bolsafeucn_back.src.Domain.Models;

namespace bolsafeucn_back.src.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<GeneralUser?> GetByEmailAsync(string email);
        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> ExistsByRutAsync(string rut);
        Task<bool> CreateUserAsync(GeneralUser user, string password, string role);
        Task<bool> CreateStudentAsync(Student student);
        Task<bool> CreateIndividualAsync(Individual individual);
        Task<bool> CreateCompanyAsync(Company company);
        Task<bool> CreateAdminAsync(Admin admin, bool superAdmin);
        Task<IEnumerable<GeneralUser>> GetAllAsync();
        Task<GeneralUser?> GetByIdAsync(int id);
        Task<GeneralUser> AddAsync(GeneralUser usuario);
        Task<bool> DeleteAsync(int id);
    }
}
