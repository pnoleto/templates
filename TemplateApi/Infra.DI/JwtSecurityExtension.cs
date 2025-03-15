using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Hosting;
using System.Security.Cryptography;
using System.Text;

namespace Infra.DI
{
    public static class JwtSecurityExtension
    {
        public static IServiceCollection AddJwtDefinitions(this IServiceCollection services, IHostApplicationBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder.Configuration);

            string? SecretKeyHash = builder.Configuration.GetRequiredSection("JwtSettings:SecretKeyHash").Get<string>();

            ArgumentNullException.ThrowIfNull(SecretKeyHash);

            services.AddAuthentication()
            .AddJwtBearer("JwtScheme", jwtOptions =>
            {
                jwtOptions.Audience = builder.Configuration["JwtSettings:Audience"];
                jwtOptions.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(SHA256.HashData(Encoding.Default.GetBytes(SecretKeyHash))),
                    ValidAudiences = builder.Configuration.GetRequiredSection("JwtSettings:ValidAudiences").Get<string[]>(),
                    ValidIssuers = builder.Configuration.GetRequiredSection("JwtSettings:ValidIssuers").Get<string[]>(),
                    ClockSkew = TimeSpan.FromSeconds(0)
                };
                jwtOptions.MapInboundClaims = false;
            });

            services.AddAuthorizationBuilder()
             .AddPolicy("JwtScheme", policy =>
                 policy.AddAuthenticationSchemes("JwtScheme")
                 .RequireAuthenticatedUser());

            return services;
        }
    }
}
