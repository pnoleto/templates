using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.ApplicationStatus.DependencyInjection;
using Microsoft.AspNetCore.Routing;
using HealthChecks.UI.Client;

namespace Infra.DI
{
    public static class HealthCheckExtension
    {
        public static IHealthChecksBuilder CheckPostgreSql(this IHealthChecksBuilder builder, IConfiguration configuration, string connectionStringName)
        {
            string? connectionString = configuration.GetConnectionString(connectionStringName);

            ArgumentNullException.ThrowIfNull(configuration);

            ArgumentNullException.ThrowIfNull(connectionString);

            return builder.AddNpgSql(connectionString, name: "postgres");
        }

        public static IHealthChecksBuilder CheckUris(this IHealthChecksBuilder builder, IConfiguration configuration, IEnumerable<Uri> Uris)
        {
            ArgumentNullException.ThrowIfNull(configuration);

            return builder.AddUrlGroup(Uris, name: "external_uris");
        }

        public static IHealthChecksBuilder CheckSystem(this IHealthChecksBuilder builder)
        {
            builder.AddDiskStorageHealthCheck(options => options.WithCheckAllDrives(), name: "disk_storage");
            builder.AddApplicationStatus(name: "application");

            return builder;
        }

        public static IEndpointConventionBuilder UseHealthCheckEndpoint(this IEndpointRouteBuilder services)
        {
            return services.MapHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                ResultStatusCodes =
                {
                    [HealthStatus.Healthy] = StatusCodes.Status200OK,
                    [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError,
                    [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
                },
                AllowCachingResponses = false
            });
        }
    }
}
