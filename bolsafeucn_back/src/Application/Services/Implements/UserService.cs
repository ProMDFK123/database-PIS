using bolsafe_ucn.src.Application.Services.Interfaces;
using bolsafeucn_back.src.Application.DTOs.AuthDTOs;
using bolsafeucn_back.src.Application.Services.Interfaces;
using bolsafeucn_back.src.Domain.Models;
using bolsafeucn_back.src.Infrastructure.Repositories.Interfaces;
using Mapster;

namespace bolsafeucn_back.src.Application.Services.Implements
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;
        private readonly IVerificationCodeRepository _verificationCodeRepository;

        public UserService(
            IUserRepository userRepository,
            IVerificationCodeRepository verificationCodeRepository,
            IEmailService emailService,
            ITokenService tokenService
        )
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _verificationCodeRepository = verificationCodeRepository;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Registra un nuevo estudiante en el sistema.
        /// </summary>
        /// <param name="registerStudentDTO">DTO con la información del estudiante</param>
        /// <param name="httpContext">Contexto HTTP</param>
        /// <returns>Mensaje de éxito o error</returns>
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
                "Applicant"
            );
            if (result == false)
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
            //Envio email de verificacion
            string code = new Random().Next(100000, 999999).ToString();
            VerificationCode verificationCode = new VerificationCode
            {
                Code = code,
                TipoCodigo = CodeType.EmailConfirmation,
                UsuarioGenericoId = user.Id,
                Expiracion = DateTime.UtcNow.AddHours(1),
            };
            var newCode = await _verificationCodeRepository.CreateCodeAsync(verificationCode);
            if (newCode == null)
            {
                throw new Exception("Error al crear el código de verificación.");
            }
            await _emailService.SendVerificationEmailAsync(user.Email!, newCode.Code);
            return "Usuario registrado exitosamente. Por favor, verifica tu correo electrónico.";
        }

        /// <summary>
        /// Registra un nuevo usuario particular en el sistema.
        /// </summary>
        /// <param name="registerIndividualDTO">Dto de registro del usuario particular</param>
        /// <param name="httpContext">Contexto HTTP</param>
        /// <returns>Mensaje de éxito o error</returns>
        public async Task<string> RegisterIndividualAsync(
            RegisterIndividualDTO registerIndividualDTO,
            HttpContext httpContext
        )
        {
            bool registrado = await _userRepository.ExistsByEmailAsync(registerIndividualDTO.Email);
            if (registrado)
            {
                throw new Exception("El correo electrónico ya está en uso.");
            }
            registrado = await _userRepository.ExistsByRutAsync(registerIndividualDTO.Rut);
            if (registrado)
            {
                throw new Exception("El RUT ya está en uso.");
            }
            var user = registerIndividualDTO.Adapt<GeneralUser>();
            user.PhoneNumber = NormalizePhoneNumber(registerIndividualDTO.Telefono);
            var result = await _userRepository.CreateUserAsync(
                user,
                registerIndividualDTO.Contraseña,
                "Offerent"
            );
            if (!result)
            {
                throw new Exception("Error al crear el usuario.");
            }
            var individual = registerIndividualDTO.Adapt<Individual>();
            individual.UsuarioGenericoId = user.Id;
            result = await _userRepository.CreateIndividualAsync(individual);
            if (!result)
            {
                throw new Exception("Error al crear el particular.");
            }
            //Envio email de verificacion
            string code = new Random().Next(100000, 999999).ToString();
            VerificationCode verificationCode = new VerificationCode
            {
                Code = code,
                TipoCodigo = CodeType.EmailConfirmation,
                UsuarioGenericoId = user.Id,
                Expiracion = DateTime.UtcNow.AddHours(1),
            };
            var newCode = await _verificationCodeRepository.CreateCodeAsync(verificationCode);
            if (newCode == null)
            {
                throw new Exception("Error al crear el código de verificación.");
            }
            await _emailService.SendVerificationEmailAsync(user.Email!, newCode.Code);
            return "Usuario registrado exitosamente. Por favor, verifica tu correo electrónico.";
        }

        public async Task<string> RegisterCompanyAsync(
            RegisterCompanyDTO registerCompanyDTO,
            HttpContext httpContext
        )
        {
            bool registrado = await _userRepository.ExistsByEmailAsync(registerCompanyDTO.Email);
            if (registrado)
            {
                throw new Exception("El correo electrónico ya está en uso.");
            }
            registrado = await _userRepository.ExistsByRutAsync(registerCompanyDTO.Rut);
            if (registrado)
            {
                throw new Exception("El RUT ya está en uso.");
            }
            var user = registerCompanyDTO.Adapt<GeneralUser>();
            user.PhoneNumber = NormalizePhoneNumber(registerCompanyDTO.Telefono);
            var result = await _userRepository.CreateUserAsync(
                user,
                registerCompanyDTO.Contraseña,
                "Offerent"
            );
            if (!result)
            {
                throw new Exception("Error al crear el usuario.");
            }
            var company = registerCompanyDTO.Adapt<Company>();
            company.UsuarioGenericoId = user.Id;
            result = await _userRepository.CreateCompanyAsync(company);
            if (!result)
            {
                throw new Exception("Error al crear la empresa.");
            }
            //Envio email de verificacion
            string code = new Random().Next(100000, 999999).ToString();
            VerificationCode verificationCode = new VerificationCode
            {
                Code = code,
                TipoCodigo = CodeType.EmailConfirmation,
                UsuarioGenericoId = user.Id,
                Expiracion = DateTime.UtcNow.AddHours(1),
            };
            var newCode = await _verificationCodeRepository.CreateCodeAsync(verificationCode);
            if (newCode == null)
            {
                throw new Exception("Error al crear el código de verificación.");
            }
            await _emailService.SendVerificationEmailAsync(user.Email!, newCode.Code);
            return "Usuario registrado exitosamente. Por favor, verifica tu correo electrónico.";
        }

        public async Task<string> RegisterAdminAsync(
            RegisterAdminDTO registerAdminDTO,
            HttpContext httpContext
        )
        {
            bool registrado = await _userRepository.ExistsByEmailAsync(registerAdminDTO.Email);
            if (registrado)
            {
                throw new Exception("El correo electrónico ya está en uso.");
            }
            registrado = await _userRepository.ExistsByRutAsync(registerAdminDTO.Rut);
            if (registrado)
            {
                throw new Exception("El RUT ya está en uso.");
            }
            var user = registerAdminDTO.Adapt<GeneralUser>();
            user.PhoneNumber = NormalizePhoneNumber(registerAdminDTO.Telefono);
            string role = "Admin";
            if (registerAdminDTO.SuperAdmin)
            {
                role = "SuperAdmin";
            }
            var result = await _userRepository.CreateUserAsync(
                user,
                registerAdminDTO.Contraseña,
                role
            );
            if (!result)
            {
                throw new Exception("Error al crear el usuario.");
            }
            var admin = registerAdminDTO.Adapt<Admin>();
            admin.UsuarioGenericoId = user.Id;
            result = await _userRepository.CreateAdminAsync(admin, registerAdminDTO.SuperAdmin);
            if (!result)
            {
                throw new Exception("Error al crear el administrador.");
            }
            //Envio email de verificacion
            string code = new Random().Next(100000, 999999).ToString();
            VerificationCode verificationCode = new VerificationCode
            {
                Code = code,
                TipoCodigo = CodeType.EmailConfirmation,
                UsuarioGenericoId = user.Id,
                Expiracion = DateTime.UtcNow.AddHours(1),
            };
            var newCode = await _verificationCodeRepository.CreateCodeAsync(verificationCode);
            if (newCode == null)
            {
                throw new Exception("Error al crear el código de verificación.");
            }
            await _emailService.SendVerificationEmailAsync(user.Email!, newCode.Code);
            return "Usuario registrado exitosamente. Por favor, verifica tu correo electrónico.";
        }

        public async Task<string> VerifyEmailAsync(
            VerifyEmailDTO verifyEmailDTO,
            HttpContext httpContext
        )
        {
            var user = await _userRepository.GetByEmailAsync(verifyEmailDTO.Email);
            if (user == null)
            {
                throw new Exception("El usuario no existe.");
            }
            if (user.EmailConfirmed)
            {
                return "El correo electrónico ya ha sido verificado.";
            }
            CodeType type = CodeType.EmailConfirmation;
            var verificationCode = await _verificationCodeRepository.GetByLatestUserIdAsync(
                user.Id,
                type
            );
            if (
                verificationCode.Code != verifyEmailDTO.VerificationCode
                || DateTime.UtcNow >= verificationCode.Expiracion
            )
            {
                int attempsCountUpdated = await _verificationCodeRepository.IncreaseAttemptsAsync(
                    user.Id,
                    type
                );
                if (attempsCountUpdated >= 5)
                {
                    bool codeDeleteResult = await _verificationCodeRepository.DeleteByUserIdAsync(
                        user.Id,
                        type
                    );
                    if (codeDeleteResult)
                    {
                        bool userDeleteResult = await _userRepository.DeleteAsync(user.Id);
                        if (userDeleteResult)
                        {
                            throw new Exception(
                                "Se ha alcanzado el límite de intentos. El usuario ha sido eliminado."
                            );
                        }
                    }
                }
                if (DateTime.UtcNow >= verificationCode.Expiracion)
                {
                    throw new Exception("El código de verificación ha expirado.");
                }
                else
                {
                    throw new Exception(
                        $"El código de verificación es incorrecto, quedan {5 - attempsCountUpdated} intentos."
                    );
                }
            }
            bool emailConfirmed = await _userRepository.ConfirmEmailAsync(user.Email!);
            if (emailConfirmed)
            {
                bool codeDeleteResult = await _verificationCodeRepository.DeleteByUserIdAsync(
                    user.Id,
                    type
                );
                if (codeDeleteResult)
                {
                    await _emailService.SendWelcomeEmailAsync(user.Email!);
                    return "!Ya puedes iniciar sesión!";
                }
                throw new Exception("Error al confirmar el correo electrónico.");
            }
            throw new Exception("Error al verificar el correo electrónico.");
        }

        public async Task<string> LoginAsync(LoginDTO loginDTO, HttpContext httpContext)
        {
            var user = await _userRepository.GetByEmailAsync(loginDTO.Email);
            if (user == null)
            {
                throw new Exception("Credenciales inválidas.");
            }
            if (!user.EmailConfirmed)
            {
                throw new Exception(
                    "Por favor, verifica tu correo electrónico antes de iniciar sesión."
                );
            }
            var result = await _userRepository.CheckPasswordAsync(user, loginDTO.Password);
            if (!result)
            {
                throw new Exception("Credenciales inválidas.");
            }
            var role = await _userRepository.GetRoleAsync(user);
            return _tokenService.CreateToken(user, role, loginDTO.RememberMe);
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
