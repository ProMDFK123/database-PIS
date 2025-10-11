using bolsafeucn_back.src.Application.DTOs;
using bolsafeucn_back.src.Application.Services.Interfaces;
using bolsafeucn_back.src.Domain.Models;
using bolsafeucn_back.src.Infrastructure.Repositories.Interfaces;

namespace bolsafeucn_back.src.Application.Services.Implements
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repo;

        public UsuarioService(IUsuarioRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<GeneralUser>> GetUsuariosAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<GeneralUser?> GetUsuarioAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<GeneralUser> CrearUsuarioAsync(UsuarioDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> EliminarUsuarioAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}
