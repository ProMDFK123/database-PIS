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

        public async Task<IEnumerable<Usuario>> GetUsuariosAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Usuario?> GetUsuarioAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<Usuario> CrearUsuarioAsync(UsuarioDto dto)
        {
            var usuario = new Usuario { Nombre = dto.Nombre, Correo = dto.Correo };

            return await _repo.AddAsync(usuario);
        }

        public async Task<bool> EliminarUsuarioAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}
