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
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository userRepository,
            IVerificationCodeRepository verificationCodeRepository,
            IEmailService emailService,
            ITokenService tokenService,
            ILogger<UserService> logger
        )
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _verificationCodeRepository = verificationCodeRepository;
            _tokenService = tokenService;
            _logger = logger;
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
            _logger.LogInformation(
                "Iniciando registro de estudiante con email: {Email}",
                registerStudentDTO.Email
            );

            bool registrado = await _userRepository.ExistsByEmailAsync(registerStudentDTO.Email);
            if (registrado)
            {
                _logger.LogWarning(
                    "Intento de registro con email duplicado: {Email}",
                    registerStudentDTO.Email
                );
                throw new Exception("El correo electrónico ya está en uso.");
            }
            registrado = await _userRepository.ExistsByRutAsync(registerStudentDTO.Rut);
            if (registrado)
            {
                _logger.LogWarning(
                    "Intento de registro con RUT duplicado: {Rut}",
                    registerStudentDTO.Rut
                );
                throw new Exception("El RUT ya está en uso.");
            }
            var user = registerStudentDTO.Adapt<GeneralUser>();
            user.PhoneNumber = NormalizePhoneNumber(registerStudentDTO.PhoneNumber);
            var result = await _userRepository.CreateUserAsync(
                user,
                registerStudentDTO.Password,
                "Applicant"
            );
            if (result == false)
            {
                _logger.LogError(
                    "Error al crear usuario estudiante con email: {Email}",
                    registerStudentDTO.Email
                );
                throw new Exception("Error al crear el usuario.");
            }
            var student = registerStudentDTO.Adapt<Student>();
            student.GeneralUserId = user.Id;
            result = await _userRepository.CreateStudentAsync(student);
            if (!result)
            {
                _logger.LogError(
                    "Error al crear perfil de estudiante para usuario ID: {UserId}",
                    user.Id
                );
                throw new Exception("Error al crear el estudiante.");
            }
            //Envio email de verificacion
            string code = new Random().Next(100000, 999999).ToString();
            VerificationCode verificationCode = new VerificationCode
            {
                Code = code,
                CodeType = CodeType.EmailConfirmation,
                GeneralUserId = user.Id,
                Expiration = DateTime.UtcNow.AddHours(1),
            };
            var newCode = await _verificationCodeRepository.CreateCodeAsync(verificationCode);
            if (newCode == null)
            {
                _logger.LogError(
                    "Error al crear código de verificación para usuario ID: {UserId}",
                    user.Id
                );
                throw new Exception("Error al crear el código de verificación.");
            }

            _logger.LogInformation("Enviando email de verificación a: {Email}", user.Email);
            await _emailService.SendVerificationEmailAsync(user.Email!, newCode.Code);
            _logger.LogInformation(
                "Estudiante registrado exitosamente con ID: {UserId}, Email: {Email}",
                user.Id,
                user.Email
            );
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
            _logger.LogInformation(
                "Iniciando registro de particular con email: {Email}",
                registerIndividualDTO.Email
            );

            bool registrado = await _userRepository.ExistsByEmailAsync(registerIndividualDTO.Email);
            if (registrado)
            {
                _logger.LogWarning(
                    "Intento de registro particular con email duplicado: {Email}",
                    registerIndividualDTO.Email
                );
                throw new Exception("El correo electrónico ya está en uso.");
            }
            registrado = await _userRepository.ExistsByRutAsync(registerIndividualDTO.Rut);
            if (registrado)
            {
                _logger.LogWarning(
                    "Intento de registro particular con RUT duplicado: {Rut}",
                    registerIndividualDTO.Rut
                );
                throw new Exception("El RUT ya está en uso.");
            }
            var user = registerIndividualDTO.Adapt<GeneralUser>();
            user.PhoneNumber = NormalizePhoneNumber(registerIndividualDTO.PhoneNumber);
            var result = await _userRepository.CreateUserAsync(
                user,
                registerIndividualDTO.Password,
                "Offerent"
            );
            if (!result)
            {
                _logger.LogError(
                    "Error al crear usuario particular con email: {Email}",
                    registerIndividualDTO.Email
                );
                throw new Exception("Error al crear el usuario.");
            }
            var individual = registerIndividualDTO.Adapt<Individual>();
            individual.GeneralUserId = user.Id;
            result = await _userRepository.CreateIndividualAsync(individual);
            if (!result)
            {
                _logger.LogError(
                    "Error al crear perfil de particular para usuario ID: {UserId}",
                    user.Id
                );
                throw new Exception("Error al crear el particular.");
            }
            //Envio email de verificacion
            string code = new Random().Next(100000, 999999).ToString();
            VerificationCode verificationCode = new VerificationCode
            {
                Code = code,
                CodeType = CodeType.EmailConfirmation,
                GeneralUserId = user.Id,
                Expiration = DateTime.UtcNow.AddHours(1),
            };
            var newCode = await _verificationCodeRepository.CreateCodeAsync(verificationCode);
            if (newCode == null)
            {
                _logger.LogError(
                    "Error al crear código de verificación para usuario ID: {UserId}",
                    user.Id
                );
                throw new Exception("Error al crear el código de verificación.");
            }

            _logger.LogInformation("Enviando email de verificación a: {Email}", user.Email);
            await _emailService.SendVerificationEmailAsync(user.Email!, newCode.Code);
            _logger.LogInformation(
                "Particular registrado exitosamente con ID: {UserId}, Email: {Email}",
                user.Id,
                user.Email
            );
            return "Usuario registrado exitosamente. Por favor, verifica tu correo electrónico.";
        }

        public async Task<string> RegisterCompanyAsync(
            RegisterCompanyDTO registerCompanyDTO,
            HttpContext httpContext
        )
        {
            _logger.LogInformation(
                "Iniciando registro de empresa con email: {Email}",
                registerCompanyDTO.Email
            );

            bool registrado = await _userRepository.ExistsByEmailAsync(registerCompanyDTO.Email);
            if (registrado)
            {
                _logger.LogWarning(
                    "Intento de registro de empresa con email duplicado: {Email}",
                    registerCompanyDTO.Email
                );
                throw new Exception("El correo electrónico ya está en uso.");
            }
            registrado = await _userRepository.ExistsByRutAsync(registerCompanyDTO.Rut);
            if (registrado)
            {
                _logger.LogWarning(
                    "Intento de registro de empresa con RUT duplicado: {Rut}",
                    registerCompanyDTO.Rut
                );
                throw new Exception("El RUT ya está en uso.");
            }
            var user = registerCompanyDTO.Adapt<GeneralUser>();
            user.PhoneNumber = NormalizePhoneNumber(registerCompanyDTO.PhoneNumber);
            var result = await _userRepository.CreateUserAsync(
                user,
                registerCompanyDTO.Password,
                "Offerent"
            );
            if (!result)
            {
                _logger.LogError(
                    "Error al crear usuario empresa con email: {Email}",
                    registerCompanyDTO.Email
                );
                throw new Exception("Error al crear el usuario.");
            }
            var company = registerCompanyDTO.Adapt<Company>();
            company.GeneralUserId = user.Id;
            result = await _userRepository.CreateCompanyAsync(company);
            if (!result)
            {
                _logger.LogError(
                    "Error al crear perfil de empresa para usuario ID: {UserId}",
                    user.Id
                );
                throw new Exception("Error al crear la empresa.");
            }
            //Envio email de verificacion
            string code = new Random().Next(100000, 999999).ToString();
            VerificationCode verificationCode = new VerificationCode
            {
                Code = code,
                CodeType = CodeType.EmailConfirmation,
                GeneralUserId = user.Id,
                Expiration = DateTime.UtcNow.AddHours(1),
            };
            var newCode = await _verificationCodeRepository.CreateCodeAsync(verificationCode);
            if (newCode == null)
            {
                _logger.LogError(
                    "Error al crear código de verificación para usuario ID: {UserId}",
                    user.Id
                );
                throw new Exception("Error al crear el código de verificación.");
            }

            _logger.LogInformation("Enviando email de verificación a: {Email}", user.Email);
            await _emailService.SendVerificationEmailAsync(user.Email!, newCode.Code);
            _logger.LogInformation(
                "Empresa registrada exitosamente con ID: {UserId}, Email: {Email}",
                user.Id,
                user.Email
            );
            return "Usuario registrado exitosamente. Por favor, verifica tu correo electrónico.";
        }

        public async Task<string> RegisterAdminAsync(
            RegisterAdminDTO registerAdminDTO,
            HttpContext httpContext
        )
        {
            _logger.LogInformation(
                "Iniciando registro de admin con email: {Email}, SuperAdmin: {SuperAdmin}",
                registerAdminDTO.Email,
                registerAdminDTO.SuperAdmin
            );

            bool registrado = await _userRepository.ExistsByEmailAsync(registerAdminDTO.Email);
            if (registrado)
            {
                _logger.LogWarning(
                    "Intento de registro de admin con email duplicado: {Email}",
                    registerAdminDTO.Email
                );
                throw new Exception("El correo electrónico ya está en uso.");
            }
            registrado = await _userRepository.ExistsByRutAsync(registerAdminDTO.Rut);
            if (registrado)
            {
                _logger.LogWarning(
                    "Intento de registro de admin con RUT duplicado: {Rut}",
                    registerAdminDTO.Rut
                );
                throw new Exception("El RUT ya está en uso.");
            }
            var user = registerAdminDTO.Adapt<GeneralUser>();
            user.PhoneNumber = NormalizePhoneNumber(registerAdminDTO.PhoneNumber);
            string role = "Admin";
            if (registerAdminDTO.SuperAdmin)
            {
                role = "SuperAdmin";
            }
            var result = await _userRepository.CreateUserAsync(
                user,
                registerAdminDTO.Password,
                role
            );
            if (!result)
            {
                _logger.LogError(
                    "Error al crear usuario admin con email: {Email}",
                    registerAdminDTO.Email
                );
                throw new Exception("Error al crear el usuario.");
            }
            var admin = registerAdminDTO.Adapt<Admin>();
            admin.GeneralUserId = user.Id;
            result = await _userRepository.CreateAdminAsync(admin, registerAdminDTO.SuperAdmin);
            if (!result)
            {
                _logger.LogError(
                    "Error al crear perfil de admin para usuario ID: {UserId}",
                    user.Id
                );
                throw new Exception("Error al crear el administrador.");
            }
            //Envio email de verificacion
            string code = new Random().Next(100000, 999999).ToString();
            VerificationCode verificationCode = new VerificationCode
            {
                Code = code,
                CodeType = CodeType.EmailConfirmation,
                GeneralUserId = user.Id,
                Expiration = DateTime.UtcNow.AddHours(1),
            };
            var newCode = await _verificationCodeRepository.CreateCodeAsync(verificationCode);
            if (newCode == null)
            {
                _logger.LogError(
                    "Error al crear código de verificación para usuario ID: {UserId}",
                    user.Id
                );
                throw new Exception("Error al crear el código de verificación.");
            }

            _logger.LogInformation("Enviando email de verificación a: {Email}", user.Email);
            await _emailService.SendVerificationEmailAsync(user.Email!, newCode.Code);
            _logger.LogInformation(
                "Admin registrado exitosamente con ID: {UserId}, Email: {Email}, Rol: {Role}",
                user.Id,
                user.Email,
                role
            );
            return "Usuario registrado exitosamente. Por favor, verifica tu correo electrónico.";
        }

        public async Task<string> VerifyEmailAsync(
            VerifyEmailDTO verifyEmailDTO,
            HttpContext httpContext
        )
        {
            _logger.LogInformation("Intentando verificar email: {Email}", verifyEmailDTO.Email);

            var user = await _userRepository.GetByEmailAsync(verifyEmailDTO.Email);
            if (user == null)
            {
                _logger.LogWarning(
                    "Intento de verificación para email no existente: {Email}",
                    verifyEmailDTO.Email
                );
                throw new Exception("El usuario no existe.");
            }
            if (user.EmailConfirmed)
            {
                _logger.LogInformation("Email ya verificado: {Email}", verifyEmailDTO.Email);
                return "El correo electrónico ya ha sido verificado.";
            }
            CodeType type = CodeType.EmailConfirmation;
            var verificationCode = await _verificationCodeRepository.GetByLatestUserIdAsync(
                user.Id,
                type
            );
            if (
                verificationCode.Code != verifyEmailDTO.VerificationCode
                || DateTime.UtcNow >= verificationCode.Expiration
            )
            {
                int attempsCountUpdated = await _verificationCodeRepository.IncreaseAttemptsAsync(
                    user.Id,
                    type
                );
                _logger.LogWarning(
                    "Intento de verificación fallido para usuario ID: {UserId}, Intentos: {Attempts}",
                    user.Id,
                    attempsCountUpdated
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
                            _logger.LogWarning(
                                "Usuario eliminado por exceder intentos de verificación. Email: {Email}, ID: {UserId}",
                                user.Email,
                                user.Id
                            );
                            throw new Exception(
                                "Se ha alcanzado el límite de intentos. El usuario ha sido eliminado."
                            );
                        }
                    }
                }
                if (DateTime.UtcNow >= verificationCode.Expiration)
                {
                    _logger.LogWarning(
                        "Código de verificación expirado para usuario ID: {UserId}",
                        user.Id
                    );
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
                    _logger.LogInformation(
                        "Email verificado exitosamente para usuario ID: {UserId}, Email: {Email}",
                        user.Id,
                        user.Email
                    );
                    await _emailService.SendWelcomeEmailAsync(user.Email!);
                    return "!Ya puedes iniciar sesión!";
                }
                _logger.LogError(
                    "Error al eliminar código de verificación para usuario ID: {UserId}",
                    user.Id
                );
                throw new Exception("Error al confirmar el correo electrónico.");
            }
            _logger.LogError("Error al confirmar email para usuario ID: {UserId}", user.Id);
            throw new Exception("Error al verificar el correo electrónico.");
        }

        public async Task<string> LoginAsync(LoginDTO loginDTO, HttpContext httpContext)
        {
            _logger.LogInformation("Intento de login para email: {Email}", loginDTO.Email);

            var user = await _userRepository.GetByEmailAsync(loginDTO.Email);
            if (user == null)
            {
                _logger.LogWarning(
                    "Intento de login con email no registrado: {Email}",
                    loginDTO.Email
                );
                throw new Exception("Credenciales inválidas.");
            }
            if (!user.EmailConfirmed)
            {
                _logger.LogWarning(
                    "Intento de login con email no verificado: {Email}, UserId: {UserId}",
                    loginDTO.Email,
                    user.Id
                );
                throw new Exception(
                    "Por favor, verifica tu correo electrónico antes de iniciar sesión."
                );
            }
            var result = await _userRepository.CheckPasswordAsync(user, loginDTO.Password);
            if (!result)
            {
                _logger.LogWarning(
                    "Intento de login con contraseña incorrecta para usuario: {Email}, UserId: {UserId}",
                    loginDTO.Email,
                    user.Id
                );
                throw new Exception("Credenciales inválidas.");
            }
            var role = await _userRepository.GetRoleAsync(user);
            _logger.LogInformation(
                "Login exitoso para usuario: {Email}, UserId: {UserId}, Role: {Role}",
                loginDTO.Email,
                user.Id,
                role
            );
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
