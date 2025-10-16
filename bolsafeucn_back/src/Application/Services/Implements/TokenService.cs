using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using bolsafe_ucn.src.Application.Services.Interfaces;
using bolsafeucn_back.src.Domain.Models;
using Microsoft.IdentityModel.Tokens;

namespace bolsafeucn_back.src.Application.Services.Implements
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<TokenService> _logger;
        private readonly string _jwtSecret;

        public TokenService(IConfiguration configuration, ILogger<TokenService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _jwtSecret = _configuration.GetValue<string>("Jwt:Key")!;
        }

        public string CreateToken(GeneralUser user, string roleName, bool rememberMe)
        {
            try
            {
                _logger.LogInformation(
                    "Creando token JWT para usuario ID: {UserId}, Email: {Email}, Role: {Role}, RememberMe: {RememberMe}",
                    user.Id,
                    user.Email,
                    roleName,
                    rememberMe
                );

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, roleName),
                    new Claim(ClaimTypes.Email, user.Email!),
                };

                var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtSecret));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var expirationHours = rememberMe ? 24 : 1;
                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddHours(expirationHours),
                    signingCredentials: creds
                );

                _logger.LogInformation(
                    "Token JWT creado exitosamente para usuario ID: {UserId}, expira en {Hours} horas",
                    user.Id,
                    expirationHours
                );
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear token JWT para usuario ID: {UserId}", user.Id);
                throw new InvalidOperationException("Error creando el token JWT.", ex);
            }
        }
    }
}
