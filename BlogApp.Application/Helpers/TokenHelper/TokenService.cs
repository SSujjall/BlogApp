using BlogApp.Application.Exceptions;
using BlogApp.Domain.Configs;
using BlogApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BlogApp.Application.Helpers.TokenHelper
{
    public class TokenService : ITokenService
    {
        private readonly JwtConfig _jwtConfig;
        private readonly UserManager<Users> _userManager;

        public TokenService(IOptions<JwtConfig> jwtSettings, UserManager<Users> userManager)
        {
            _jwtConfig = jwtSettings.Value;
            _userManager = userManager;
        }

        public async Task<string> GenerateJwtToken(Users user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var userRole = _userManager.GetRolesAsync(user).Result.FirstOrDefault();

            var claims = new List<Claim>
            {
                new Claim("UserId", user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, userRole)
            };

            var token = new JwtSecurityToken(
                issuer: _jwtConfig.ValidIssuer,
                audience: _jwtConfig.ValidAudience,
                expires: DateTime.Now.AddMinutes(15), // 15 minutes expiration for JWT Token
                claims: claims,
                signingCredentials: credentials
            );

            return await Task.Run(() => new JwtSecurityTokenHandler().WriteToken(token));
        }

        public async Task<string> GenerateRefreshToken()
        {
            var randomNumber = new byte[128];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return await Task.FromResult(Convert.ToBase64String(randomNumber));
        }

        public ClaimsPrincipal? GetTokenPrincipal(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _jwtConfig.ValidIssuer,
                ValidAudience = _jwtConfig.ValidAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret)),
                ValidateLifetime = false, // Don't validate expiration, we check expiration manually

                // Enforce algorithm here instead of manual check
                ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 }
            };
            
            try
            {
                var principal = tokenHandler.ValidateToken(
                    token,
                    tokenValidationParameters,
                    out var validateJwtToken
                );

                if (validateJwtToken is not JwtSecurityToken jwtToken)
                {
                    return null;
                }

                return principal;
            }
            catch (SecurityTokenException ex)
            {
                var message = ex switch
                {
                    SecurityTokenExpiredException => "Access token has expired.",
                    SecurityTokenInvalidSignatureException => "Token signature is invalid.",
                    SecurityTokenInvalidAudienceException => "Token audience is invalid.",
                    SecurityTokenInvalidIssuerException => "Token issuer is invalid.",
                    _ => "Token validation failed."
                };
                throw new ServiceException(
                    new Dictionary<string, string> { { "Token", message } },
                    HttpStatusCode.Unauthorized
                );
            }
        }
    }
}
