using Infra.DI;

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
     .AddExceptionHandler()
     .AddCorsDefinitions()
     .AddScheduledJobs()
     .AddRepositories()
     .AddFeedRobots()
     .AddMediator()
     .AddLogging();

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

app.MapControllers();
app.Run();
