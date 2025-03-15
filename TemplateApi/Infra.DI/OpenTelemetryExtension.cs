using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Infra.DI
{
    public static class OpenTelemetryExtension
    {
        public static IServiceCollection AddOpenTelemetryInstrumentation(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration);

            services.AddOpenTelemetry()
              .ConfigureResource(resource => resource.AddService(
                      serviceName: configuration.GetValue("Instrumentation:ServiceName", string.Empty), 
                      serviceVersion: configuration.GetValue("Instrumentation:ServiceVersion", string.Empty))
                 .AddAttributes(
                     new Dictionary<string, object>
                     {
                         { "environnment" , configuration.GetValue("ASPCORE_ENVIRONMENT", string.Empty) },
                     }
                 .AsEnumerable()))
              .WithTracing(tracing => tracing.AddAspNetCoreInstrumentation()
                  .AddConsoleExporter()
                  .AddOtlpExporter(cfg => cfg.Endpoint = new Uri(configuration.GetValue("Instrumentation:Uri", string.Empty))))
              .WithMetrics(metrics => metrics.AddAspNetCoreInstrumentation()
                  .AddConsoleExporter()
                  .AddOtlpExporter(cfg => cfg.Endpoint = new Uri(configuration.GetValue("Instrumentation:Uri", string.Empty))));

            return services;
        }

        public static IHostApplicationBuilder AddOpenTelemetryLogger(this IHostApplicationBuilder builder)
        {
            builder.Logging.AddOpenTelemetry(options => options.SetResourceBuilder(ResourceBuilder
                .CreateDefault()
                .AddService(
                    serviceName: builder.Configuration
                    .GetValue("Instrumentation:ServiceName", string.Empty),
                    serviceVersion: builder.Configuration
                    .GetValue("Instrumentation:ServiceVersion", string.Empty)))
            .AddConsoleExporter()
            .AddOtlpExporter(cfg => cfg.Endpoint = new Uri(builder.Configuration.GetValue("Instrumentation:Uri", string.Empty))));

            return builder;
        }
    }
}
