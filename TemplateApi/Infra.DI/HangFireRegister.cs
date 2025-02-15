using Hangfire;
using Hangfire.Dashboard;
using Hangfire.PostgreSql;
using Infra.RecurringJobs;
using System.Security.Principal;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.DI
{
    public class HangRireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            IIdentity? identity = context.GetHttpContext().User.Identity;

            if(identity is null) return true;

            return identity.IsAuthenticated;
        }
    }

    public static class HangFireRegister
    {
        public static IServiceCollection AddScheduledJobs(this IServiceCollection services) => 
            services.AddTransient<FeedsJob>();

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

        public static IApplicationBuilder UseProtectedHangFireDashboard(this IApplicationBuilder builder) => 
            builder.UseHangfireDashboard("/hangfire/dashboard",
            options: new DashboardOptions
            {
                Authorization = [new HangRireAuthorizationFilter()]
            });

        public static IApplicationBuilder UseScheduledJobs(this IApplicationBuilder app)
        {
            IRecurringJobManager recurringJobManager = app.ApplicationServices.GetRequiredService<IRecurringJobManager>();

            recurringJobManager.AddOrUpdate<FeedsJob>(nameof(FeedsJob.ExecuteAsync), (feedsJob) => feedsJob
                    .ExecuteAsync(new CancellationTokenSource().Token), Cron.Daily(11,41));

            return app;
        }
    }
}
