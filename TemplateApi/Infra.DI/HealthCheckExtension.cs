using Infra.Integrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Infra.DI
{
    public static class HealthCheckExtension
    {
        private static async Task<HealthCheckResult> CheckGoogle(IHealthChecksBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder.Services);

            IGoogleClient googleClient = builder.Services.BuildServiceProvider().GetRequiredService<IGoogleClient>();

            try
            {
                await googleClient.Get();
            }
            catch
            {
                HealthCheckResult.Unhealthy("Coldn't connect");
            }

            return HealthCheckResult.Healthy("checked succesful");
        }

        private static async Task<HealthCheckResult> CheckDb<TContext>(IHealthChecksBuilder builder) where TContext: DbContext
        {
            ArgumentNullException.ThrowIfNull(builder.Services);

            TContext dbContext = builder.Services.BuildServiceProvider().GetRequiredService<TContext>();

            bool canConnect = await dbContext.Database.CanConnectAsync();

            if (!canConnect) HealthCheckResult.Unhealthy("Coldn't connect to database");

            return HealthCheckResult.Healthy("Db checked succesful");
        }

        private static async Task<HealthCheckResult> CheckHosts(IHealthChecksBuilder builder, string hostsSectionName)
        {
            ArgumentNullException.ThrowIfNull(builder.Services);

            IConfiguration configuration = builder.Services.BuildServiceProvider().GetRequiredService<IConfiguration>();

            ArgumentNullException.ThrowIfNull(configuration);

            HttpClient httpClient = builder.Services.BuildServiceProvider().GetRequiredService<HttpClient>();

            ArgumentNullException.ThrowIfNull(httpClient);

            string[] hosts = configuration.GetRequiredSection(hostsSectionName).Get<string[]>();

            ArgumentNullException.ThrowIfNull(hosts);

            try
            {
                foreach (string host in hosts)
                {
                    HttpResponseMessage response = await httpClient.GetAsync(host);
                }
            }
            catch
            {
                return HealthCheckResult.Healthy("Hosts check error");
            }

            return HealthCheckResult.Healthy("Db checked succesful");
        }

        public static IHealthChecksBuilder AddCheckDatabase<TContext>(this IHealthChecksBuilder builder) where TContext : DbContext
        {
            return builder.AddAsyncCheck(nameof(TContext), () => CheckDb<TContext>(builder), ["Database", "Status"]);
        }

        public static IHealthChecksBuilder AddCheckHosts(this IHealthChecksBuilder builder, string requiredHostsSection)
        {
            return builder.AddAsyncCheck(nameof(requiredHostsSection), ()=> CheckHosts(builder, requiredHostsSection), ["Hosts", "Endpoints", "Integrated", "Services"]);
        }

        public static IHealthChecksBuilder AddCheckGoogle(this IHealthChecksBuilder builder)
        {
            return builder.AddAsyncCheck("Google", () => CheckGoogle(builder), ["Hosts", "Endpoints", "Google", "Rest", "HttpClient"]);
        }
    }
}
