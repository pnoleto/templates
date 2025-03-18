using MediatR;
using System.Text;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Application.Results;
using Application.Events;
using Shared;

namespace Application.Handlers
{
    public class LoginHandler(JwtSettings jwtSettings) : IRequestHandler<LoginEvent, LoginResult>, IRequestHandler<RenewAccessTokenEvent, LoginResult>{

        private ClaimsPrincipal ValidateToken(RenewAccessTokenEvent request, JwtSecurityTokenHandler tokenHandler)
        {
            return tokenHandler.ValidateToken(
                request.RefreshToken,
                new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidAudience = jwtSettings.Audiences[0],
                    IssuerSigningKey = new SymmetricSecurityKey(SHA256.HashData(Encoding.Default.GetBytes(jwtSettings.SecretKeyHash))),
                    ClockSkew = TimeSpan.FromSeconds(0),
                    RequireExpirationTime = true,
                    RequireSignedTokens = true,
                    ValidateLifetime = true,
                },
            out _);
        }

        private Task<string> GenerateToken(ClaimsPrincipal claimsIdentity, DateTime expirationDate)
        {
            JwtSecurityTokenHandler tokenHandler = new();
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Issuer = jwtSettings.Audiences[0],
                Audience = jwtSettings.Audiences[0],
                IssuedAt = DateTime.UtcNow,
                Subject = (ClaimsIdentity)claimsIdentity.Identity,
                Expires = expirationDate,
                SigningCredentials = new(
                    new SymmetricSecurityKey(SHA256.HashData(Encoding.Default.GetBytes(jwtSettings.SecretKeyHash))),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);

            return Task.FromResult(tokenHandler.WriteToken(token));
        }

        private static ClaimsPrincipal GenerateClaims(LoginEvent request)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(
            [
               new(ClaimTypes.Name, request.Username),
               new(ClaimTypes.Role, "Admin")
            ]));
        }

        public async Task<LoginResult> Handle(LoginEvent request, CancellationToken cancellationToken)
        {
            ClaimsPrincipal claims = GenerateClaims(request);

            return new LoginResult
            {
                AccessToken = await GenerateToken(claims, DateTime.UtcNow.AddHours(2)),
                RefreshToken = await GenerateToken(claims, DateTime.UtcNow.AddDays(2))
            };
        }

        public async Task<LoginResult> Handle(RenewAccessTokenEvent request, CancellationToken cancellationToken)
        {
            ClaimsPrincipal claims = ValidateToken(request, new JwtSecurityTokenHandler());

            return new LoginResult
            {
                AccessToken = await GenerateToken(claims, DateTime.UtcNow.AddHours(2)),
                RefreshToken = request.RefreshToken
            };
        }

    }
}
