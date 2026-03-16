using DocuSense.Application.Dtos.AuthenticationDto;
using DocuSense.Application.Services.Authentication;
using DocuSense.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DocuSense.Infrastructure.Services.Authentication
{
    public class TokenGenerator : ITokenService
    {
        private readonly IOptions<JwtSettings> _options;

        public TokenGenerator(IOptions<JwtSettings> options)
        {
            _options = options;
        }

        public TokenDto GenerateAccessToken(string userId, string email)
        {
            var issuer = _options.Value.Issuer;
            var audience = _options.Value.Audience;
            var key = _options.Value.Key;
            var expires = _options.Value.ExpiresMinutes;

            if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience) || string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException("Token yapılandırılırken hata oluştu.");
            }

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub,userId),
                new Claim(ClaimTypes.Email,email),
                new Claim(ClaimTypes.NameIdentifier,userId),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expires),
                signingCredentials: signingCredentials
                );

            JwtSecurityTokenHandler tokenHandler = new();

            return new TokenDto
            {
                AccessToken = tokenHandler.WriteToken(token),
                Expiration = DateTime.UtcNow.AddMinutes(expires),
            };
        }
    }
}