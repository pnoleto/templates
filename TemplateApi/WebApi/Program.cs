using Infra.Database.ModelDbContext;
using Infra.DI;

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
             .AddLogging();

        builder.Services
            .AddHealthChecks()
            .AddCheckDatabase<NewsDbContext>()
            .AddCheckHosts("requiredHosts")
            .AddCheckGoogle();

        WebApplication app = builder.Build();

        if (app.Environment.IsDevelopment())
            app.UseSwaggerUIDefinitions();

        app.UseExceptionHandler()
            .UseHttpsRedirection()
            .UseAuthentication()
            .UseAuthorization()
            .UseProtectedHangFireDashboard()
            .UseScheduledJobs()
            .UseCors();

        app.MapHealthChecks("/health");
        app.MapControllers();
        app.Run();
    }
}