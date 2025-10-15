using System.ComponentModel.DataAnnotations;

namespace bolsafeucn_back.src.Application.DTOs.AuthDTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El correo no es válido")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "El campo RememberMe es obligatorio")]
        public required bool RememberMe { get; set; }
    }
}
