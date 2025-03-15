using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.DI
{
    public static class CorsExtension
    {
        public static IServiceCollection AddCorsDefinitions(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddCors(cors =>
                cors.AddDefaultPolicy(policy =>
                    policy.WithHeaders(configuration.GetRequiredSection("Cors:Headers").Get<string[]>() ?? [])
                        .WithOrigins(configuration.GetRequiredSection("Cors:Origins").Get<string[]>() ?? [])
                        .WithMethods(configuration.GetRequiredSection("Cors:Methods").Get<string[]>() ?? [])));
        }
    }
}
