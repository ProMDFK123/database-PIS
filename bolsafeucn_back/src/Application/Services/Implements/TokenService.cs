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

        private readonly string _jwtSecret;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            _jwtSecret = _configuration.GetValue<string>("Jwt:Key")!;
        }

        public string CreateToken(GeneralUser user, string roleName, bool rememberMe)
        {
            try
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, roleName),
                    new Claim(ClaimTypes.Email, user.Email!),
                };

                var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtSecret));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddHours(rememberMe ? 24 : 1),
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error creando el token JWT.", ex);
            }
        }
    }
}
