using bolsafeucn_back.src.Application.DTOs;
using bolsafeucn_back.src.Application.Services.Interfaces;
using bolsafeucn_back.src.Domain.Models;
using bolsafeucn_back.src.Infrastructure.Repositories.Interfaces;
using Mapster;

namespace bolsafeucn_back.src.Application.Services.Implements
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Registra un nuevo estudiante en el sistema.
        /// </summary>
        /// <param name="registerStudentDTO">DTO con la información del estudiante</param>
        /// <param name="httpContext">Contexto HTTP</param>
        /// <returns>Mensaje de éxito o error</returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> RegisterStudentAsync(
            RegisterStudentDTO registerStudentDTO,
            HttpContext httpContext
        )
        {
            bool registrado = await _userRepository.ExistsByEmailAsync(registerStudentDTO.Email);
            if (registrado)
            {
                throw new Exception("El correo electrónico ya está en uso.");
            }
            registrado = await _userRepository.ExistsByRutAsync(registerStudentDTO.Rut);
            if (registrado)
            {
                throw new Exception("El RUT ya está en uso.");
            }
            var user = registerStudentDTO.Adapt<GeneralUser>();
            user.PhoneNumber = NormalizePhoneNumber(registerStudentDTO.Telefono);
            var result = await _userRepository.CreateUserAsync(
                user,
                registerStudentDTO.Contraseña,
                "Estudiante"
            );
            if (!result)
            {
                throw new Exception("Error al crear el usuario.");
            }
            var student = registerStudentDTO.Adapt<Student>();
            student.UsuarioGenericoId = user.Id;
            result = await _userRepository.CreateStudentAsync(student);
            if (!result)
            {
                throw new Exception("Error al crear el estudiante.");
            }
            return "Usuario registrado exitosamente.";
        }

        /*public async Task<IEnumerable<GeneralUser>> GetUsuariosAsync()
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
        }*/
        public string NormalizePhoneNumber(string phoneNumber)
        {
            var digits = new string(phoneNumber.Where(char.IsDigit).ToArray());
            return "+56 " + digits;
        }
    }
}
