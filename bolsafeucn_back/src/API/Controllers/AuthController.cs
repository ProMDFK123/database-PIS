using bolsafeucn_back.src.Application.DTOs.AuthDTOs;
using bolsafeucn_back.src.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace bolsafeucn_back.src.API.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IUserService _service;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserService userService, ILogger<AuthController> logger)
        {
            _service = userService;
            _logger = logger;
        }

        [HttpPost("register/student")]
        public async Task<IActionResult> Register([FromBody] RegisterStudentDTO registerStudentDTO)
        {
            _logger.LogInformation(
                "Attempting to register new student with email {Email}",
                registerStudentDTO.Email
            );
            var message = await _service.RegisterStudentAsync(registerStudentDTO, HttpContext);
            return Ok(new { message });
        }

        [HttpPost("register/individual")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterIndividualDTO registerIndividualDTO
        )
        {
            _logger.LogInformation(
                "Endpoint: POST /api/auth/register/individual - Intentando registrar particular con email: {Email}",
                registerIndividualDTO.Email
            );
            var message = await _service.RegisterIndividualAsync(
                registerIndividualDTO,
                HttpContext
            );
            return Ok(new { message });
        }

        [HttpPost("register/company")]
        public async Task<IActionResult> Register([FromBody] RegisterCompanyDTO registerCompanyDTO)
        {
            _logger.LogInformation(
                "Endpoint: POST /api/auth/register/company - Intentando registrar empresa con email: {Email}",
                registerCompanyDTO.Email
            );
            var message = await _service.RegisterCompanyAsync(registerCompanyDTO, HttpContext);
            return Ok(new { message });
        }

        [HttpPost("register/admin")]
        public async Task<IActionResult> Register([FromBody] RegisterAdminDTO registerAdminDTO)
        {
            _logger.LogInformation(
                "Endpoint: POST /api/auth/register/admin - Intentando registrar admin con email: {Email}",
                registerAdminDTO.Email
            );
            var message = await _service.RegisterAdminAsync(registerAdminDTO, HttpContext);
            return Ok(new { message });
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailDTO verifyEmailDTO)
        {
            _logger.LogInformation(
                "Endpoint: POST /api/auth/verify-email - Intentando verificar email: {Email}",
                verifyEmailDTO.Email
            );
            var message = await _service.VerifyEmailAsync(verifyEmailDTO, HttpContext);
            return Ok(new { message });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            _logger.LogInformation(
                "Endpoint: POST /api/auth/login - Intento de login para: {Email}",
                loginDTO.Email
            );
            var token = await _service.LoginAsync(loginDTO, HttpContext);
            return Ok(new { token });
        }

        /*
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usuarios = await _service.GetUsuariosAsync();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuario = await _service.GetUsuarioAsync(id);
            if (usuario == null)
                return NotFound();
            return Ok(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UsuarioDto dto)
        {
            var usuario = await _service.CrearUsuarioAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, usuario);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.EliminarUsuarioAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }
        */
    }
}
