using System.ComponentModel.DataAnnotations;

namespace bolsafeucn_back.src.Application.DTOs.AuthDTOs
{
    public class VerifyEmailDTO
    {
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El correo no es válido")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "El código de verificación es obligatorio")]
        [RegularExpression(
            @"^\d{6}$",
            ErrorMessage = "El código de verificación debe tener 6 dígitos."
        )]
        public required string VerificationCode { get; set; }
    }
}
