using MediatR;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Application.Results;
using Application.Events;
using Shared;
using Application.Handlers.Base;
using FluentValidation;

namespace Application.Handlers
{
    public class RenewAccesTokenHandler :
        LoginHandlerBase<RenewAccessTokenEvent>,
        IRequestHandler<RenewAccessTokenEvent, LoginResult>{
        public RenewAccesTokenHandler(JwtSettings jwtSettings, JwtSecurityTokenHandler tokenHandler) : base(jwtSettings, tokenHandler)
        {
            RuleFor(entity => entity.RefreshToken)
                .NotNull()
                .NotEmpty()
                .MinimumLength(3);
        }

        public async Task<LoginResult> Handle(RenewAccessTokenEvent request, CancellationToken cancellationToken)
        {
            await this.ValidateAndThrowAsync(request, cancellationToken);

            ClaimsPrincipal claims = ValidateToken(request);

            return new LoginResult
            {
                AccessToken = await GenerateToken(claims, DateTime.UtcNow.AddHours(2)),
                RefreshToken = request.RefreshToken
            };
        }

    }
}
