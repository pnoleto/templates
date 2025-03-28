using MediatR;
using System.Security.Claims;
using Application.Results;
using Application.Events;
using Shared;
using Application.Handlers.Base;
using System.IdentityModel.Tokens.Jwt;
using FluentValidation;

namespace Application.Handlers
{
    public class LoginHandler :
        LoginHandlerBase<LoginEvent>,
        IRequestHandler<LoginEvent, LoginResult>{
        public LoginHandler(JwtSettings jwtSettings, JwtSecurityTokenHandler tokenHandler) : base(jwtSettings, tokenHandler)
        {
            RuleFor(entity => entity.Username)
                .NotNull()
                .NotEmpty()
                .MinimumLength(3);

            RuleFor(entity => entity.Password)
                .NotNull()
                .NotEmpty()
                .MinimumLength(3);
        }

        public async Task<LoginResult> Handle(LoginEvent request, CancellationToken cancellationToken)
        {
            await this.ValidateAndThrowAsync(request, cancellationToken);

            ClaimsPrincipal claims = GenerateClaims(request);

            return new LoginResult
            {
                AccessToken = await GenerateToken(claims, DateTime.UtcNow.AddHours(2)),
                RefreshToken = await GenerateToken(claims, DateTime.UtcNow.AddDays(2))
            };
        }
    }
}
