using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared;

namespace Infra.DI
{
    public static class ConfigurationExtension
    {
        private static OpentelemetrySettings LoadOpenTelemetryConfig(this IHostApplicationBuilder builder)
        {
            return new()
            {
                Environment = builder.Configuration.GetValue("Instrumentation:Environment", string.Empty),
                ServiceName = builder.Configuration.GetValue("Instrumentation:ServiceName", string.Empty),
                ServiceVersion = builder.Configuration.GetValue("Instrumentation:ServiceVersion", string.Empty),
                Uri = builder.Configuration.GetValue("Instrumentation:Uri", string.Empty)
            };
        }

        private static JwtSettings LoadJwtConfig(this IHostApplicationBuilder builder)
        {
            return new()
            {
                SecretKeyHash = builder.Configuration.GetValue("JwtSettings:SecretKeyHash", string.Empty),
                Audiences = builder.Configuration.GetSection("JwtSettings:Audiences").Get<string[]>() ?? [],
                Issuers = builder.Configuration.GetSection("JwtSettings:Issuers").Get<string[]>() ?? []
            };
        }

        private static CorsSettings LoadCorsConfig(this IHostApplicationBuilder builder)
        {
            return new()
            {
                Headers = builder.Configuration.GetSection("Cors:Headers").Get<string[]>() ?? [],
                Methods = builder.Configuration.GetSection("Cors:Methods").Get<string[]>() ?? [],
                Origins = builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? []
            };
        }

        public static IHostApplicationBuilder AddConfigurationItems(this IHostApplicationBuilder builder)
        {
            builder.Services
            .AddSingleton(config => builder.LoadOpenTelemetryConfig())
            .AddSingleton(config => builder.LoadCorsConfig())
            .AddSingleton(config => builder.LoadJwtConfig());

            return builder;
        }
    }
}
