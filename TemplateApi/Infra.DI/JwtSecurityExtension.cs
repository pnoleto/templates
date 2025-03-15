using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infra.DI
{
    public static class JwtSecurityExtension
    {
        public static IServiceCollection AddJwtDefinitions(this IServiceCollection services, IHostApplicationBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder.Configuration);

            services.AddAuthentication()
            .AddJwtBearer("JwtScheme", jwtOptions =>
            {
                jwtOptions.MetadataAddress = builder.Configuration["JwtSettings:MetadataAddress"] ?? string.Empty;
                // Optional if the MetadataAddress is specified
                jwtOptions.Authority = builder.Configuration["JwtSettings:Authority"];
                jwtOptions.Audience = builder.Configuration["JwtSettings:Audience"];
                jwtOptions.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
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
