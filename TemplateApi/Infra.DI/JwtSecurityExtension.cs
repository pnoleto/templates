using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Hosting;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Infra.DI
{
    public static class JwtSecurityExtension
    {
        private const string DefaultScheme = "JwtScheme";

        public static IServiceCollection AddJwtDefinitions(this IServiceCollection services, IHostApplicationBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder.Configuration);

            string? secretKeyHash = builder.Configuration.GetRequiredSection("JwtSettings:SecretKeyHash").Get<string>();

            ArgumentNullException.ThrowIfNull(secretKeyHash);

            services.AddAuthentication(DefaultScheme)
            .AddJwtBearer(DefaultScheme, jwtOptions =>
            {
                jwtOptions.Audience = builder.Configuration["JwtSettings:Audience"];
                jwtOptions.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(SHA256.HashData(Encoding.Default.GetBytes(secretKeyHash))),
                    ValidAudiences = builder.Configuration.GetRequiredSection("JwtSettings:ValidAudiences").Get<string[]>(),
                    ValidIssuers = builder.Configuration.GetRequiredSection("JwtSettings:ValidIssuers").Get<string[]>(),
                    ClockSkew = TimeSpan.FromSeconds(0)
                };
                jwtOptions.MapInboundClaims = false;
            });

            services.AddAuthorizationBuilder()
             .AddPolicy(DefaultScheme, policy =>
                 policy.AddAuthenticationSchemes(DefaultScheme)
                 .RequireAuthenticatedUser());

            return services;
        }

    }
}
