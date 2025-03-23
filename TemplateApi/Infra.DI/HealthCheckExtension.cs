using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.ApplicationStatus.DependencyInjection;

namespace Infra.DI
{
    public static class HealthCheckExtension
    {
        public static IHealthChecksBuilder CheckSqlServer(this IHealthChecksBuilder builder, IConfiguration configuration, string connectionStringName)
        {
            string? connectionString = configuration.GetConnectionString(connectionStringName);

            ArgumentNullException.ThrowIfNull(configuration);

            ArgumentNullException.ThrowIfNull(connectionString);

            return builder.AddSqlServer(connectionString, name: "sql_server");
        }

        public static IHealthChecksBuilder CheckUris(this IHealthChecksBuilder builder, IConfiguration configuration, IEnumerable<Uri> Uris)
        {
            ArgumentNullException.ThrowIfNull(configuration);

            return builder.AddUrlGroup(Uris, name: "external_uris");
        }

        public static IHealthChecksBuilder CheckSystem(this IHealthChecksBuilder builder)
        {
            return builder.AddDiskStorageHealthCheck(options => 
            options.WithCheckAllDrives(), name: "disk_storage")
                .AddApplicationStatus(name: "application");
        }

        public static IServiceCollection AddHealthCheckUI(this IServiceCollection services)
        {
            services.AddHealthChecksUI(opt =>
             opt.SetEvaluationTimeInSeconds(10)
                .MaximumHistoryEntriesPerEndpoint(10)
                .SetApiMaxActiveRequests(1))
            .AddInMemoryStorage();

            return services;
        }

        public static IApplicationBuilder UseHealthCheckEndpoint(this IApplicationBuilder services)
        {
            return services.UseHealthChecks("/health", new HealthCheckOptions
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
