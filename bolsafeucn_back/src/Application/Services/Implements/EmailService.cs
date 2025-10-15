using bolsafeucn_back.src.Application.Services.Interfaces;
using Resend;

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
        }

        /// <summary>
        /// Carga una plantilla de correo electrónico y reemplaza el marcador de posición {{CODE}} con el código proporcionado si es necesario.
        /// </summary>
        /// <param name="templateName">Nombre de la plantilla a cargar.</param>
        /// <param name="code">El código de verificación a insertar en la plantilla.</param>
        /// <returns>El contenido HTML de la plantilla con el código insertado si fuese así el caso.</returns>
        public async Task<string> LoadTemplateAsync(string templateName, string? code)
        {
            var templatePath = Path.Combine(
                _environment.ContentRootPath,
                "src",
                "Application",
                "Templates",
                "Emails",
                $"{templateName}.html"
            );
            var htmlContent = await File.ReadAllTextAsync(templatePath);
            return htmlContent.Replace("{{CODE}}", code);
        }
    }
}
