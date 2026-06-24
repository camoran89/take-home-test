using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Fundo.Applications.WebApi.Services
{
    public class JwtTokenService : ITokenService
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expiresMinutes;

        public JwtTokenService(IConfiguration configuration)
        {
            _key = configuration["Jwt:Key"] ?? "ThisIsASecretJwtSigningKeyForLocalDevelopment123!";
            _issuer = configuration["Jwt:Issuer"] ?? "Fundo.Api";
            _audience = configuration["Jwt:Audience"] ?? "Fundo.Client";
            _expiresMinutes = int.TryParse(configuration["Jwt:ExpiresMinutes"], out var value) ? value : 60;
        }

        public string GenerateToken(string username)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Sub, username)
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_expiresMinutes),
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
