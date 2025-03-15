using Infra.DI;

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
             .AddRepositories()
             .AddHttpClient()
             .AddFeedRobots()
             .AddMediator()
             .AddHealthUI()
             .AddLogging();

        builder.Services.AddHealthChecks()
            .CheckSqlServer(builder.Configuration,"LocalDb")
            .CheckSystem();

        WebApplication app = builder.Build();

        if (app.Environment.IsDevelopment())
            app.UseProtectedHangFireDashboard()
                .UseSwaggerUIDefinitions()
                .UseHealthUI();

        app.UseExceptionHandler()
            .UseHttpsRedirection()
            .UseAuthentication()
            .UseAuthorization()
            .UseScheduledJobs()
            .UseCors()
            .UseHsts();

        app.MapControllers();
        app.Run();
    }
}