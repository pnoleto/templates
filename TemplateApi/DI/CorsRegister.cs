using Microsoft.Extensions.DependencyInjection;

namespace Infra.DI
{
    public static class CorsRegister
    {
        public static IServiceCollection AddCorsDefinitions(this IServiceCollection services) =>
            services.AddCors(cors =>
                cors.AddDefaultPolicy(policy =>
                policy.AllowAnyHeader()
                    .AllowAnyOrigin()
                    .AllowAnyMethod()));
    }
}
