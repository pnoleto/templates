using HealthChecks.UI.Client;
using Infra.DI;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

internal class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.AddOpenTelemetryLogger();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer()
             .AddSwaggerDefinitions(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name)
             //.ExecuteMigrationsOnStartup(builder.Configuration, "LocaDb")
             .AddSqlServerDbContext(builder.Configuration, "LocaDb")
             .AddOpenTelemetryInstrumentation(builder.Configuration)
             .AddCorsDefinitions(builder.Configuration)
             .AddHangFireSchedulerWithInMemoryDb()
             .AddJwtDefinitions(builder)
             .AddHttpCLientFactory()
             .AddExceptionHandler()
             .AddScheduledJobs()
             .AddHealthCheckUI()
             .AddRepositories()
             .AddHttpClient()
             .AddFeedRobots()
             .AddMediator()
             .AddLogging();

        builder.Services.AddHealthChecks()
            .CheckSqlServer(builder.Configuration, "LocalDb")
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