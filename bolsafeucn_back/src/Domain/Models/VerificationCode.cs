namespace bolsafeucn_back.src.Domain.Models
{
    /// <summary>
    /// Tipos de códigos de verificación.
    /// </summary>
    public enum CodeType
    {
        EmailConfirmation,
        PasswordReset,
    }

    /// <summary>
    /// Clase que representa un código de verificación para acciones como confirmación de correo o restablecimiento de contraseña.
    /// </summary>
    public class VerificationCode
    {
        public int Id { get; set; }
        public required string Code { get; set; }
        public required CodeType TipoCodigo { get; set; }
        public required int UsuarioGenericoId { get; set; }
        public int Intentos { get; set; } = 0;
        public required DateTime Expiracion { get; set; }
        public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    }
}
