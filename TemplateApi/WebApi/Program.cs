using Infra.DI;
using System.Reflection;

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
     .AddKeyCloak(builder.Configuration)
     .AddOpenTelemetryInstrumentation()
     .AddExceptionHandler()
     .AddCorsDefinitions()
     .AddJwtDefinitions()
     .AddScheduledJobs()
     .AddRepositories()
     .AddHttpClient()
     .AddFeedRobots()
     .AddMediator()
     .AddLogging();

builder.Services
    .AddHealthChecks()
    .CheckSystem()
    .CheckSqlServer(builder.Configuration, "NewsConnection");

builder.Services.AddHealthCheckUI();

WebApplication app = builder.Build();

app.UseRouting()
    .UseAuthentication()
    .UseAuthorization()
    .UseExceptionHandler()
    .UseHttpsRedirection()
    .UseScheduledJobs()
    .UseCors()
    .UseHsts();

app.UseHealthCheckEndpoint();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseProtectedHangFireDashboard();
    app.UseSwaggerDefinitions();
    app.UseHealthCheckUIEndpoint();
}

await app.RunAsync();
