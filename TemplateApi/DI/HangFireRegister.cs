using Hangfire;
using Hangfire.Dashboard;
using Hangfire.PostgreSql;
using Infra.RecurringJobs;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.DI
{
    public class HangRireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context) => true;//context.GetHttpContext().User.Identity.IsAuthenticated;
    }
    public static class HangFireRegister
    {
        public static IServiceCollection AddRangFireSchedulerWithPostgreSql(this IServiceCollection services) => services
            .AddTransient<FeedsJob>()
            .AddHangfireServer()
            .AddHangfire(options => options
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseColouredConsoleLogProvider()
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage((cfg) =>
                {
                    var serviceProvider = services.BuildServiceProvider();
                    cfg.UseNpgsqlConnection(serviceProvider.GetRequiredService<IConfiguration>().GetConnectionString("NewsConnection"));
                }));

        public static IServiceCollection AddRangFireSchedulerWithInMemoryDb(this IServiceCollection services) => services
            .AddTransient<FeedsJob>()
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
            var recurringJobManager = app.ApplicationServices.GetRequiredService<IRecurringJobManager>();

            recurringJobManager.AddOrUpdate<FeedsJob>(nameof(FeedsJob.Execute), (feedsJob) => feedsJob.Execute(new CancellationTokenSource().Token), Cron.Daily());

            return app;
        }
    }
}
