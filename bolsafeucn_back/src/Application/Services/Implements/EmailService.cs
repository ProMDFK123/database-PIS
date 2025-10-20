using bolsafeucn_back.src.Application.Services.Interfaces;
using Resend;
using Serilog;

namespace bolsafeucn_back.src.Application.Services.Implements
{
    public class EmailService : IEmailService
    {
        private readonly IResend _resend;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public EmailService(
            IResend resend,
            IConfiguration configuration,
            IWebHostEnvironment environment
        )
        {
            _resend = resend;
            _configuration = configuration;
            _environment = environment;
        }

        /// <summary>
        /// Envía un correo de verificación al email proporcionado con el código dado.
        /// </summary>
        /// <param name="email">El correo electrónico del usuario.</param>
        /// <param name="code">El código de verificación generado.</param>
        /// <returns></returns>
        public async Task SendVerificationEmailAsync(string email, string code)
        {
            try
            {
                Log.Information("Iniciando envío de email de verificación a: {Email}", email);
                var htmlBody = await LoadTemplateAsync("VerificationEmail", code);
                var message = new EmailMessage
                {
                    To = email,
                    From = _configuration.GetValue<string>("EmailConfiguration:From")!,
                    Subject = _configuration.GetValue<string>(
                        "EmailConfiguration:VerificationSubject"
                    )!,
                    HtmlBody = htmlBody,
                };
                var result = await _resend.EmailSendAsync(message);
                if (!result.Success)
                {
                    Log.Error("El envío del email de verificación falló para: {Email}", email);
                    throw new Exception("Error al enviar el correo de verificación.");
                }
                Log.Information("Email de verificación enviado exitosamente a: {Email}", email);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al enviar email de verificación a: {Email}", email);
                throw;
            }
        }

        public async Task SendResetPasswordVerificationEmailAsync(string email, string code)
        {
            try
            {
                Log.Information("Iniciando envío de email de verificación a: {Email}", email);
                var htmlBody = await LoadTemplateAsync("PasswordResetEmail", code);
                var message = new EmailMessage
                {
                    To = email,
                    From = _configuration.GetValue<string>("EmailConfiguration:From")!,
                    Subject = _configuration.GetValue<string>(
                        "EmailConfiguration:PasswordResetSubject"
                    )!,
                    HtmlBody = htmlBody,
                };
                await _resend.EmailSendAsync(message);
                Log.Information("Email de verificación enviado exitosamente a: {Email}", email);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al enviar email de verificación a: {Email}", email);
                throw;
            }
        }

        /// <summary>
        /// Envía un correo de bienvenida al email proporcionado.
        /// </summary>
        /// <param name="email">El correo electrónico del usuario.</param>
        /// <returns></returns>
        public async Task SendWelcomeEmailAsync(string email)
        {
            try
            {
                Log.Information("Iniciando envío de email de bienvenida a: {Email}", email);
                var htmlBody = await LoadTemplateAsync("WelcomeEmail", null);
                var message = new EmailMessage
                {
                    From = _configuration.GetValue<string>("EmailConfiguration:From")!,
                    To = email,
                    Subject = _configuration.GetValue<string>("EmailConfiguration:WelcomeSubject")!,
                    HtmlBody = htmlBody,
                };
                await _resend.EmailSendAsync(message);
                Log.Information("Email de bienvenida enviado exitosamente a: {Email}", email);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al enviar email de bienvenida a: {Email}", email);
                throw;
            }
        }

        /// <summary>
        /// Carga una plantilla de correo electrónico y reemplaza el marcador de posición {{CODE}} con el código proporcionado si es necesario.
        /// </summary>
        /// <param name="templateName">Nombre de la plantilla a cargar.</param>
        /// <param name="code">El código de verificación a insertar en la plantilla.</param>
        /// <returns>El contenido HTML de la plantilla con el código insertado si fuese así el caso.</returns>
        public async Task<string> LoadTemplateAsync(string templateName, string? code)
        {
            try
            {
                var templatePath = Path.Combine(
                    _environment.ContentRootPath,
                    "src",
                    "Application",
                    "Templates",
                    "Emails",
                    $"{templateName}.html"
                );
                Log.Debug(
                    "Cargando template de email: {TemplateName} desde {Path}",
                    templateName,
                    templatePath
                );
                var htmlContent = await File.ReadAllTextAsync(templatePath);
                return htmlContent.Replace("{{CODE}}", code);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al cargar template de email: {TemplateName}", templateName);
                throw;
            }
        }
    }
}
