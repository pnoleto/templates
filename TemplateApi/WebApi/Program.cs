using Infra.DI;

internal class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.AddConfigurationItems();
        builder.AddOpenTelemetryLogger();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer()
             .AddSwaggerDefinitions(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name)
             .ExecuteMigrationsOnStartup(builder.Configuration, "NewConnection")
             .AddSqlServerDbContext(builder.Configuration, "NewConnection")
             .AddHangFireSchedulerWithInMemoryDb()
             .AddOpenTelemetryInstrumentation()
             .AddHttpCLientFactory()
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
            .CheckSqlServer(builder.Configuration, "NewConnection")
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