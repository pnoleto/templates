using MediatR;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Application.Results;
using Application.Events;
using Shared;
using Application.Handlers.Base;

namespace Application.Handlers
{
    public class RenewAccesTokenHandler(JwtSettings jwtSettings, JwtSecurityTokenHandler tokenHandler) :
        LoginHandlerBase<RenewAccessTokenEvent>(jwtSettings, tokenHandler),
        IRequestHandler<RenewAccessTokenEvent, LoginResult>{

        public async Task<LoginResult> Handle(RenewAccessTokenEvent request, CancellationToken cancellationToken)
        {
            ClaimsPrincipal claims = ValidateToken(request);

            return new LoginResult
            {
                AccessToken = await GenerateToken(claims, DateTime.UtcNow.AddHours(2)),
                RefreshToken = request.RefreshToken
            };
        }

    }
}
