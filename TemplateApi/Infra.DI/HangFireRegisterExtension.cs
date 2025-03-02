using Hangfire;
using Hangfire.Dashboard;
using Hangfire.PostgreSql;
using Infra.RecurringJobs;
using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.DI
{
    partial class HangRireAuthorizationFilter() : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            ClaimsPrincipal user = context.GetHttpContext().User;

            if (user == null || user.Identity is null) return false;

            IEnumerable<string> userRoles = user.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value);

            return user.Identity.IsAuthenticated && userRoles.Contains("Admin");
        }
    }

    public static class HangFireRegisterExtension
    {
        private static IGlobalConfiguration SetHangFireDefaultConfigs(this IGlobalConfiguration hangFireOptions)
        {
            return hangFireOptions.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                            .UseColouredConsoleLogProvider()
                            .UseSimpleAssemblyNameTypeSerializer()
                            .UseRecommendedSerializerSettings();
        }

        public static IServiceCollection AddHangFireSchedulerWithPostgreSql(this IServiceCollection services, string connectionString) => services
            .AddHangfireServer()
            .AddHangfire(options => options.SetHangFireDefaultConfigs()
                .UsePostgreSqlStorage(pgOptions => pgOptions.UseNpgsqlConnection(connectionString)));

        public static IServiceCollection AddHangFireSchedulerWithSqlServer(this IServiceCollection services, string connectionString) => services
            .AddHangfireServer()
            .AddHangfire(options => options.SetHangFireDefaultConfigs()
                .UseSqlServerStorage(connectionString));

        public static IServiceCollection AddHangFireSchedulerWithInMemoryDb(this IServiceCollection services) => services
            .AddHangfireServer()
            .AddHangfire(options => options.SetHangFireDefaultConfigs()
                .UseInMemoryStorage());

        public static IApplicationBuilder UseProtectedHangFireDashboard(this IApplicationBuilder builder, string hangFireEndpoint = "/hangfire/dashboard")
        {
            return builder.UseHangfireDashboard(hangFireEndpoint,
                options: new DashboardOptions
                {
                    Authorization = [new HangRireAuthorizationFilter()]
                });
        }

        public static IServiceCollection AddScheduledJobs(this IServiceCollection services) => services
            .AddTransient<FeedsJob>();

        public static IApplicationBuilder UseScheduledJobs(this IApplicationBuilder app)
        {
            IRecurringJobManager recurringJobManager = app.ApplicationServices
                .GetRequiredService<IRecurringJobManager>();

            IBackgroundJobClient backgroundJobFactory = app.ApplicationServices
                .GetRequiredService<IBackgroundJobClient>();

            recurringJobManager.AddOrUpdate<FeedsJob>(nameof(FeedsJob), recurringJob => recurringJob
                .ExecuteAsync(new CancellationTokenSource().Token), Cron.Daily(11, 00));

            return app;
        }
    }
}
