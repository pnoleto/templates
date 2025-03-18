using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Shared;

namespace Infra.DI
{
    public static class OpenTelemetryExtension
    {
        private static ResourceBuilder AddServiceInfo(this ResourceBuilder resource, OpentelemetrySettings settings)
        {
            return resource.AddService(serviceName: settings.ServiceName, serviceVersion: settings.ServiceVersion);
        }

        private static IEnumerable<KeyValuePair<string, object>> LoadAdditionalAttributes(this OpentelemetrySettings settings)
        {
            return new Dictionary<string, object> { { "environnment", settings.Environment } }.AsEnumerable();
        }
        public static IServiceCollection AddOpenTelemetryInstrumentation(this IServiceCollection services)
        {
            OpentelemetrySettings settings = services.BuildServiceProvider().GetRequiredService<OpentelemetrySettings>();

            ArgumentNullException.ThrowIfNull(settings);

            services.AddOpenTelemetry()
              .ConfigureResource(resource => resource.AddServiceInfo(settings)
                 .AddAttributes(settings.LoadAdditionalAttributes()))
              .WithTracing(tracing => tracing.AddAspNetCoreInstrumentation()
                 .AddConsoleExporter()
                 .AddOtlpExporter(cfg => cfg.Endpoint = new Uri(settings.Uri)))
              .WithMetrics(metrics => metrics.AddAspNetCoreInstrumentation()
                 .AddConsoleExporter()
                 .AddOtlpExporter(cfg => cfg.Endpoint = new Uri(settings.Uri)));

            return services;
        }

        public static IHostApplicationBuilder AddOpenTelemetryLogger(this IHostApplicationBuilder builder)
        {
            OpentelemetrySettings settings = builder.Services.BuildServiceProvider().GetRequiredService<OpentelemetrySettings>();

            builder.Logging.AddOpenTelemetry(options => options.SetResourceBuilder(
                ResourceBuilder.CreateDefault().AddServiceInfo(settings))
                .AddConsoleExporter()
                .AddOtlpExporter(cfg => cfg.Endpoint = new Uri(settings.Uri)));

            return builder;
        }
    }
}
