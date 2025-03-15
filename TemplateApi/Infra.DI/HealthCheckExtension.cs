using HealthChecks.ApplicationStatus.DependencyInjection;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            return builder.AddDiskStorageHealthCheck(options => options
                .WithCheckAllDrives(), name: "disk_storage")
             .AddApplicationStatus(name: "application");
        }

        public static IServiceCollection AddHealthUI(this IServiceCollection services)
        {
            services.AddHealthChecksUI(option=> option
                .MaximumHistoryEntriesPerEndpoint(5)
                .SetApiMaxActiveRequests(1))
             .AddInMemoryStorage(options=> options
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging());

            return services;
        }

        public static IApplicationBuilder UseHealthUI(this IApplicationBuilder services)
        {
            return services.UseRouting()
            .UseEndpoints(config =>
            {
                config.MapHealthChecksUI();
                config.MapHealthChecks("/health", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }
    }
}
