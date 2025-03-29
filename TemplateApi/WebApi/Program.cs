using Infra.DI;
using System.Reflection;

namespace WebApi
{
    static class Program
    {
        private static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            builder.AddConfigurationItems();
            builder.AddOpenTelemetryLogger();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer()
                 .AddSwaggerDefinitions(Assembly.GetExecutingAssembly().GetName().Name)
                 //.ExecuteMigrationsOnStartup(builder.Configuration, "NewsConnection")
                 .AddSqlServerDbContext(builder.Configuration, "NewsConnection")
                 .AddHttpCLientFactory(builder.Configuration)
                 .AddHangFireSchedulerWithInMemoryDb()
                 .AddOpenTelemetryInstrumentation()
                 .AddExceptionHandler()
                 .AddCorsDefinitions()
                 .AddJwtDefinitions()
                 .AddScheduledJobs()
                 .AddHealthCheckUI()
                 .AddRepositories()
                 .AddHttpClient()
                 .AddFeedRobots()
                 .AddMediator()
                 .AddLogging();

            builder.Services.AddHealthChecks()
                .CheckSqlServer(builder.Configuration, "NewsConnection")
                .CheckSystem();

            WebApplication app = builder.Build();

            app.UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseHealthCheckEndpoint();

            if (app.Environment.IsDevelopment())
                app.UseHealthChecksUI()
                    .UseSwaggerDefinitions();

            app.UseProtectedHangFireDashboard()
                .UseExceptionHandler()
                .UseHttpsRedirection()
                .UseScheduledJobs()
                .UseCors()
                .UseHsts();

            app.MapControllers();
            app.Run();
        }
    }
}