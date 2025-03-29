using System.Text;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Application.Events;
using Shared;
using FluentValidation;

namespace Application.Handlers.Base
{
    public abstract class LoginHandlerBase<T> : AbstractValidator<T> where T : class
    {
        private readonly JwtSettings jwtSettings;
        private readonly JwtSecurityTokenHandler tokenHandler;

        protected LoginHandlerBase(JwtSettings jwtSettings, JwtSecurityTokenHandler tokenHandler)
        {
            this.jwtSettings = jwtSettings;
            this.tokenHandler = tokenHandler;

            ClassLevelCascadeMode = CascadeMode.Continue;
        }

        private protected byte[] GetKey()
        {
            return SHA256.HashData(Encoding.UTF8.GetBytes(jwtSettings.SecretKeyHash));
        }

        protected static ClaimsPrincipal GenerateClaims(LoginEvent request)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(
            [
               new(ClaimTypes.Name, request.Username),
               new(ClaimTypes.Role, "Admin")
            ]));
        }

        protected ClaimsPrincipal ValidateToken(RenewAccessTokenEvent request)
        {
            return this.tokenHandler.ValidateToken(
                request.RefreshToken,
                new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidAudience = jwtSettings.Audiences[0],
                    IssuerSigningKey = new SymmetricSecurityKey(GetKey()),
                    ClockSkew = TimeSpan.FromSeconds(0),
                    RequireExpirationTime = true,
                    RequireSignedTokens = true,
                    ValidateLifetime = true,
                },
            out _);
        }

        protected Task<string> GenerateToken(ClaimsPrincipal claimsIdentity, DateTime expirationDate)
        {
            ArgumentNullException.ThrowIfNull(claimsIdentity.Identity);

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Issuer = jwtSettings.Audiences[0],
                Audience = jwtSettings.Audiences[0],
                IssuedAt = DateTime.UtcNow,
                Subject = (ClaimsIdentity)claimsIdentity.Identity,
                Expires = expirationDate,
                SigningCredentials = new(
                    new SymmetricSecurityKey(GetKey()),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);

            return Task.FromResult(tokenHandler.WriteToken(token));
        }

    }
}
