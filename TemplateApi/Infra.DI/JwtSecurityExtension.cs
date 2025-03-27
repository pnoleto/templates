using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace Infra.DI
{
    public static class JwtSecurityExtension
    {
        private const string DefaultScheme = "JwtScheme";

        private static TokenValidationParameters LoadJwtSettings(this JwtSettings settings)
        {
            return new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(SHA256.HashData(Encoding.Default.GetBytes(settings.SecretKeyHash))),
                ValidAudiences = settings.Audiences,
                ValidIssuers = settings.Issuers,
                ClockSkew = TimeSpan.FromSeconds(0)
            };
        }

        public static IServiceCollection AddJwtDefinitions(this IServiceCollection services)
        {
            JwtSettings settings = services.BuildServiceProvider()
                .GetRequiredService<JwtSettings>();

            ArgumentNullException.ThrowIfNull(settings);

            services.AddAuthentication(DefaultScheme)
            .AddJwtBearer(DefaultScheme, jwtOptions =>
            {
                jwtOptions.Audience = settings.Audiences[0];
                jwtOptions.TokenValidationParameters = settings.LoadJwtSettings();
                jwtOptions.MapInboundClaims = false;
            });

            services.AddAuthorizationBuilder()
             .AddPolicy(DefaultScheme, policy =>
                 policy.AddAuthenticationSchemes(DefaultScheme)
                 .RequireAuthenticatedUser());

            services.AddScoped<JwtSecurityTokenHandler>();

            return services;
        }
    }
}
