using bolsafeucn_back.src.Domain.Models;
using bolsafeucn_back.src.Infrastructure.Data;
using bolsafeucn_back.src.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace bolsafeucn_back.src.Infrastructure.Repositories.Implements
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<GeneralUser> _userManager;

        public UserRepository(AppDbContext context, UserManager<GeneralUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Obtiene un usuario por su correo electrónico.
        /// </summary>
        /// <param name="email">Correo electrónico</param>
        /// <returns>El usuario encontrado o null si no existe.</returns>
        public async Task<GeneralUser?> GetByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        /// <summary>
        /// Verifica si un correo electrónico ya está registrado.
        /// </summary>
        /// <param name="email">Correo electrónico</param>
        /// <returns>True si el correo electrónico ya está registrado, de lo contrario false.</returns>
        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Usuarios.AnyAsync(u => u.Email == email);
        }

        /// <summary>
        /// Verifica si un RUT ya está registrado.
        /// </summary>
        /// <param name="rut">RUT del estudiante</param>
        /// <returns>True si el RUT ya está registrado, de lo contrario false.</returns>
        public async Task<bool> ExistsByRutAsync(string rut)
        {
            return await _context.Usuarios.AnyAsync(e => e.Rut == rut);
        }

        /// <summary>
        /// Crea un nuevo usuario en el sistema.
        /// </summary>
        /// <param name="user">Usuario a crear</param>
        /// <param name="password">Contraseña del usuario</param>
        /// <returns>True si se creó el usuario, de lo contrario false.</returns>
        public async Task<bool> CreateUserAsync(GeneralUser user, string password, string role)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                var userRole = await _userManager.AddToRoleAsync(user, role);
                return userRole.Succeeded;
            }
            return false;
        }

        public async Task<bool> CreateStudentAsync(Student student)
        {
            var result = await _context.Estudiantes.AddAsync(student);
            await _context.SaveChangesAsync();
            return result != null;
        }

        public async Task<bool> CreateIndividualAsync(Individual individual)
        {
            var result = await _context.Particulares.AddAsync(individual);
            await _context.SaveChangesAsync();
            return result != null;
        }

        public async Task<bool> CreateCompanyAsync(Company company)
        {
            var result = await _context.Empresas.AddAsync(company);
            await _context.SaveChangesAsync();
            return result != null;
        }

        public async Task<bool> CreateAdminAsync(Admin admin, bool superAdmin)
        {
            var result = await _context.Administradores.AddAsync(admin);
            await _context.SaveChangesAsync();
            return result != null;
        }

        public async Task<IEnumerable<GeneralUser>> GetAllAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<GeneralUser?> GetByIdAsync(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<GeneralUser> AddAsync(GeneralUser usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return false;

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
