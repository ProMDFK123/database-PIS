using bolsafeucn_back.src.Application.Services.Interfaces;
using Resend;

namespace bolsafeucn_back.src.Application.Services.Implements
{
    public class EmailService : IEmailService
    {
        private readonly IResend _resend;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<EmailService> _logger;

        public EmailService(
            IResend resend,
            IConfiguration configuration,
            IWebHostEnvironment environment,
            ILogger<EmailService> logger
        )
        {
            _resend = resend;
            _configuration = configuration;
            _environment = environment;
            _logger = logger;
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
                _logger.LogInformation(
                    "Iniciando envío de email de verificación a: {Email}",
                    email
                );
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
                await _resend.EmailSendAsync(message);
                _logger.LogInformation(
                    "Email de verificación enviado exitosamente a: {Email}",
                    email
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar email de verificación a: {Email}", email);
                throw;
            }
        }

        public async Task SendWelcomeEmailAsync(string email)
        {
            try
            {
                _logger.LogInformation("Iniciando envío de email de bienvenida a: {Email}", email);
                var htmlBody = await LoadTemplateAsync("WelcomeEmail", null);
                var message = new EmailMessage
                {
                    From = _configuration.GetValue<string>("EmailConfiguration:From")!,
                    To = email,
                    Subject = _configuration.GetValue<string>("EmailConfiguration:WelcomeSubject")!,
                    HtmlBody = htmlBody,
                };
                await _resend.EmailSendAsync(message);
                _logger.LogInformation(
                    "Email de bienvenida enviado exitosamente a: {Email}",
                    email
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar email de bienvenida a: {Email}", email);
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
                _logger.LogDebug(
                    "Cargando template de email: {TemplateName} desde {Path}",
                    templateName,
                    templatePath
                );
                var htmlContent = await File.ReadAllTextAsync(templatePath);
                return htmlContent.Replace("{{CODE}}", code);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error al cargar template de email: {TemplateName}",
                    templateName
                );
                throw;
            }
        }
    }
}
