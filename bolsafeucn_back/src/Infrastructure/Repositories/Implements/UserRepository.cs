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
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(
            AppDbContext context,
            UserManager<GeneralUser> userManager,
            ILogger<UserRepository> logger
        )
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
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
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        /// <summary>
        /// Verifica si un RUT ya está registrado.
        /// </summary>
        /// <param name="rut">RUT del estudiante</param>
        /// <returns>True si el RUT ya está registrado, de lo contrario false.</returns>
        public async Task<bool> ExistsByRutAsync(string rut)
        {
            return await _context.Users.AnyAsync(e => e.Rut == rut);
        }

        /// <summary>
        /// Crea un nuevo usuario en el sistema.
        /// </summary>
        /// <param name="user">Usuario a crear</param>
        /// <param name="password">Contraseña del usuario</param>
        /// <returns>True si se creó el usuario, de lo contrario false.</returns>
        public async Task<bool> CreateUserAsync(GeneralUser user, string password, string role)
        {
            _logger.LogInformation(
                "Creando usuario en la base de datos: {Email}, Rol: {Role}",
                user.Email,
                role
            );
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                _logger.LogInformation(
                    "Usuario creado exitosamente: {Email}, UserId: {UserId}",
                    user.Email,
                    user.Id
                );
                var userRole = await _userManager.AddToRoleAsync(user, role);
                if (userRole.Succeeded)
                {
                    _logger.LogInformation(
                        "Rol {Role} asignado exitosamente a usuario ID: {UserId}",
                        role,
                        user.Id
                    );
                }
                else
                {
                    _logger.LogError(
                        "Error al asignar rol {Role} a usuario ID: {UserId}. Errores: {Errors}",
                        role,
                        user.Id,
                        string.Join(", ", userRole.Errors.Select(e => e.Description))
                    );
                }
                return userRole.Succeeded;
            }
            _logger.LogError(
                "Error al crear usuario {Email}. Errores: {Errors}",
                user.Email,
                string.Join(", ", result.Errors.Select(e => e.Description))
            );
            return false;
        }

        public async Task<bool> CreateStudentAsync(Student student)
        {
            _logger.LogInformation(
                "Creando perfil de estudiante para usuario ID: {UserId}",
                student.GeneralUserId
            );
            var result = await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
            _logger.LogInformation(
                "Perfil de estudiante creado exitosamente para usuario ID: {UserId}",
                student.GeneralUserId
            );
            return result != null;
        }

        public async Task<bool> CreateIndividualAsync(Individual individual)
        {
            _logger.LogInformation(
                "Creando perfil de particular para usuario ID: {UserId}",
                individual.GeneralUserId
            );
            var result = await _context.Individuals.AddAsync(individual);
            await _context.SaveChangesAsync();
            _logger.LogInformation(
                "Perfil de particular creado exitosamente para usuario ID: {UserId}",
                individual.GeneralUserId
            );
            return result != null;
        }

        public async Task<bool> CreateCompanyAsync(Company company)
        {
            _logger.LogInformation(
                "Creando perfil de empresa para usuario ID: {UserId}",
                company.GeneralUserId
            );
            var result = await _context.Companies.AddAsync(company);
            await _context.SaveChangesAsync();
            _logger.LogInformation(
                "Perfil de empresa creado exitosamente para usuario ID: {UserId}",
                company.GeneralUserId
            );
            return result != null;
        }

        public async Task<bool> CreateAdminAsync(Admin admin, bool superAdmin)
        {
            _logger.LogInformation(
                "Creando perfil de admin para usuario ID: {UserId}, SuperAdmin: {SuperAdmin}",
                admin.GeneralUserId,
                superAdmin
            );
            var result = await _context.Admins.AddAsync(admin);
            await _context.SaveChangesAsync();
            _logger.LogInformation(
                "Perfil de admin creado exitosamente para usuario ID: {UserId}",
                admin.GeneralUserId
            );
            return result != null;
        }

        public async Task<bool> ConfirmEmailAsync(string email)
        {
            _logger.LogInformation("Confirmando email en base de datos: {Email}", email);
            var result = await _context
                .Users.Where(u => u.Email == email)
                .ExecuteUpdateAsync(u => u.SetProperty(user => user.EmailConfirmed, true));

            if (result > 0)
            {
                _logger.LogInformation(
                    "Email confirmado exitosamente en base de datos: {Email}",
                    email
                );
            }
            else
            {
                _logger.LogWarning("No se pudo confirmar email en base de datos: {Email}", email);
            }
            return result > 0;
        }

        public async Task<bool> CheckPasswordAsync(GeneralUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<string> GetRoleAsync(GeneralUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles.FirstOrDefault()!;
        }

        public async Task<IEnumerable<GeneralUser>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<GeneralUser?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<GeneralUser> AddAsync(GeneralUser user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation("Intentando eliminar usuario ID: {UserId}", id);
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                _logger.LogWarning("Usuario ID: {UserId} no encontrado para eliminación", id);
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Usuario ID: {UserId} eliminado exitosamente", id);
            return true;
        }
    }
}
