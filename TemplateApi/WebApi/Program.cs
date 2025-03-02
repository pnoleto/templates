using Infra.DI;
using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddOpenTelemetryLogger();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer()
     .AddHangFireSchedulerWithInMemoryDb()
     .AddOpenTelemetryInstrumentation()
     //.ExecuteMigrationsOnStartup("LocaDb")
     .AddApiKeyAuthentication()
     .AddSwaggerDefinitions(
        xmlDocumentName: Assembly.GetExecutingAssembly().GetName().Name, 
        addApiKeyDefinitions: true
     ).AddInMemoryDbContext()
     .AddExceptionHandler()
     .AddCorsDefinitions()
     .AddProblemDetails()
     .AddScheduledJobs()
     .AddRepositories()
     .AddFeedRobots()
     .AddMediator()
     .AddLogging();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseSwaggerUIDefinitions()
        .UseExceptionHandler("/Error")
        .UseDeveloperExceptionPage();

app.UseHttpsRedirection()
    .UseAuthentication()
    .UseAuthorization()
    .UseProtectedHangFireDashboard()
    .UseScheduledJobs()
    .UseCors();

app.MapControllers();
app.Run();
