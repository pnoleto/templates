using Infra.Database.ModelDbContext;
using Infra.DI;
using System.Collections.Immutable;

internal class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.AddOpenTelemetryLogger();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer()
             .AddHangFireSchedulerWithInMemoryDb()
             .AddOpenTelemetryInstrumentation()
             //.ExecuteMigrationsOnStartup("LocaDb")
             .AddApiKeyAuthentication()
             .AddSwaggerDefinitions(
                xmlDocumentName: System.Reflection.Assembly.GetExecutingAssembly().GetName().Name,
                addApiKeyDefinitions: true
             ).AddInMemoryDbContext()
             .AddHttpCLientFactory()
             .AddExceptionHandler()
             .AddCorsDefinitions()
             .AddScheduledJobs()
             .AddRepositories()
             .AddHttpClient()
             .AddFeedRobots()
             .AddMediator()
             .AddHealthUI()
             .AddLogging();

        builder.Services
            .AddHealthChecks()
            .CheckSqlServer("LocalDb")
            .CheckSystem();

        WebApplication app = builder.Build();


        if (app.Environment.IsDevelopment())
            app.UseSwaggerUIDefinitions()
                .UseHealthUI()
                .UseProtectedHangFireDashboard();

        app.UseExceptionHandler()
            .UseHttpsRedirection()
            .UseAuthentication()
            .UseAuthorization()
            .UseScheduledJobs()
            .UseCors();

        app.MapControllers();
        app.Run();
    }
}