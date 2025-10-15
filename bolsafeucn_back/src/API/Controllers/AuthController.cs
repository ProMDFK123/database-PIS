using bolsafeucn_back.src.Application.DTOs.AuthDTOs;
using bolsafeucn_back.src.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace bolsafeucn_back.src.API.Controllers
{
    public class AuthController(IUserService userService) : BaseController
    {
        private readonly IUserService _service = userService;

        [HttpPost("register/student")]
        public async Task<IActionResult> Register([FromBody] RegisterStudentDTO registerStudentDTO)
        {
            var message = await _service.RegisterStudentAsync(registerStudentDTO, HttpContext);
            return Ok(new { message });
        }

        [HttpPost("register/individual")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterIndividualDTO registerIndividualDTO
        )
        {
            var message = await _service.RegisterIndividualAsync(
                registerIndividualDTO,
                HttpContext
            );
            return Ok(new { message });
        }

        [HttpPost("register/company")]
        public async Task<IActionResult> Register([FromBody] RegisterCompanyDTO registerCompanyDTO)
        {
            var message = await _service.RegisterCompanyAsync(registerCompanyDTO, HttpContext);
            return Ok(new { message });
        }

        [HttpPost("register/admin")]
        public async Task<IActionResult> Register([FromBody] RegisterAdminDTO registerAdminDTO)
        {
            var message = await _service.RegisterAdminAsync(registerAdminDTO, HttpContext);
            return Ok(new { message });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
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
