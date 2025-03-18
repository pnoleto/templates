using Microsoft.Extensions.DependencyInjection;
using Shared;

namespace Infra.DI
{
    public static class CorsExtension
    {
        public static IServiceCollection AddCorsDefinitions(this IServiceCollection services)
        {
            CorsSettings settings = services.BuildServiceProvider().GetRequiredService<CorsSettings>();

            return services.AddCors(cors =>
                cors.AddDefaultPolicy(policy =>
                    policy.WithHeaders(settings.Headers)
                        .WithOrigins(settings.Origins)
                        .WithMethods(settings.Methods)));
        }
    }
}
