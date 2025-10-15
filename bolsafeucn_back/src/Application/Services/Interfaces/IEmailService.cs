namespace bolsafeucn_back.src.Application.Services.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de envío de correos electrónicos.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Envia un correo electrónico para verificar la direccion de correo del usuario.
        /// </summary>
        /// <param name="email">El correo electrónico del usuario.</param>
        /// <param name="code">El código de verificación generado.</param>
        Task SendVerificationEmailAsync(string email, string code);

        /// <summary>
        /// Carga una plantilla de correo electrónico.
        /// </summary>
        /// <param name="templateName">El nombre de la plantilla a cargar.</param>
        /// <param name="code">El código de verificación a insertar en la plantilla.</param>
        /// <returns>El contenido de la plantilla cargada.</returns>
        Task<string> LoadTemplateAsync(string templateName, string code);
    }
}
