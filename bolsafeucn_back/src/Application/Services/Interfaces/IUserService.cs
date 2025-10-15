using bolsafeucn_back.src.Application.DTOs.AuthDTOs;
using bolsafeucn_back.src.Domain.Models;

namespace bolsafeucn_back.src.Application.Services.Interfaces
{
    public interface IUserService
    {
        /*
        Task<IEnumerable<GeneralUser>> GetUsuariosAsync();
        Task<GeneralUser?> GetUsuarioAsync(int id);
        Task<GeneralUser> CrearUsuarioAsync(UsuarioDto dto);
        Task<bool> EliminarUsuarioAsync(int id);
        */
        Task<string> RegisterStudentAsync(
            RegisterStudentDTO registerStudentDTO,
            HttpContext httpContext
        );
        Task<string> RegisterIndividualAsync(
            RegisterIndividualDTO registerIndividualDTO,
            HttpContext httpContext
        );
        Task<string> RegisterCompanyAsync(
            RegisterCompanyDTO registerCompanyDTO,
            HttpContext httpContext
        );
        Task<string> RegisterAdminAsync(
            RegisterAdminDTO registerAdminDTO,
            HttpContext httpContext
        );
    }
}
