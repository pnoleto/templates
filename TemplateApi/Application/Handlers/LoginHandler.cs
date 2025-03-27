using MediatR;
using System.Security.Claims;
using Application.Results;
using Application.Events;
using Shared;
using Application.Handlers.Base;
using System.IdentityModel.Tokens.Jwt;

namespace Application.Handlers
{
    public class LoginHandler(JwtSettings jwtSettings, JwtSecurityTokenHandler tokenHandler) :
        LoginHandlerBase<LoginEvent>(jwtSettings, tokenHandler),
        IRequestHandler<LoginEvent, LoginResult>{

        public async Task<LoginResult> Handle(LoginEvent request, CancellationToken cancellationToken)
        {
            ClaimsPrincipal claims = GenerateClaims(request);

            return new LoginResult
            {
                AccessToken = await GenerateToken(claims, DateTime.UtcNow.AddHours(2)),
                RefreshToken = await GenerateToken(claims, DateTime.UtcNow.AddDays(2))
            };
        }
    }
}
