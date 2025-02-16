using Hangfire;
using Hangfire.Dashboard;
using Hangfire.PostgreSql;
using Infra.RecurringJobs;
using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
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
        public static IServiceCollection AddScheduledJobs(this IServiceCollection services) => services
            .AddTransient<FeedsJob>()
            .AddTransient<MigrationsJob>();

        public static IServiceCollection AddHangFireSchedulerWithPostgreSql(this IServiceCollection services, string connectionString) => services
            .AddHangfireServer()
            .AddHangfire(hangFireOptions => hangFireOptions
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseColouredConsoleLogProvider()
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage((postgresOptions) =>
                {
                    IConfiguration configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
                    postgresOptions.UseNpgsqlConnection(configuration.GetConnectionString(connectionString));
                }));

        public static IServiceCollection AddHangFireSchedulerWithSqlServer(this IServiceCollection services, string connectionString) => services
            .AddHangfireServer()
            .AddHangfire(options => options
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseColouredConsoleLogProvider()
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(connectionString));

        public static IServiceCollection AddHangFireSchedulerWithInMemoryDb(this IServiceCollection services) => services
            .AddHangfireServer()
            .AddHangfire(options => options
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseColouredConsoleLogProvider()
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseInMemoryStorage());

        public static IApplicationBuilder UseProtectedHangFireDashboard(this IApplicationBuilder builder)
        {
            return builder.UseHangfireDashboard("/hangfire/dashboard",
                options: new DashboardOptions
                {
                    Authorization = [new HangRireAuthorizationFilter()]
                });
        }

        public static IApplicationBuilder UseScheduledJobs(this IApplicationBuilder app)
        {
            IRecurringJobManager recurringJobManager = app.ApplicationServices.GetRequiredService<IRecurringJobManager>();

            IBackgroundJobClient backgroundJobFactory = app.ApplicationServices.GetRequiredService<IBackgroundJobClient>();

            recurringJobManager.AddOrUpdate<FeedsJob>(nameof(FeedsJob.ExecuteAsync), (feedsJob) => feedsJob
                .ExecuteAsync(new CancellationTokenSource().Token), Cron.Daily(11, 00));

            backgroundJobFactory.Enqueue<MigrationsJob>(nameof(MigrationsJob.ExecuteAsync).ToLowerInvariant(), (feedsJob) => feedsJob
                .ExecuteAsync(new CancellationTokenSource().Token));

            return app;
        }
    }
}
