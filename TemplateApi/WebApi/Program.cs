using Infra.DI;
using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddOpenTelemetryLogger();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer()
     .AddHangFireSchedulerWithInMemoryDb()
     .AddOpenTelemetryInstrumentation()
     .AddApiKeyAuthentication()
     .AddSwaggerDefinitions(Assembly.GetExecutingAssembly().GetName().Name)
     .AddInMemoryDbContext()
     .AddCorsDefinitions()
     .AddScheduledJobs()
     .AddRepositories()
     .AddFeedRobots()
     .AddMediator()
     .AddLogging();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.UseSwaggerUIDefinitions();

app.UseHttpsRedirection()
    .UseAuthentication()
    .UseAuthorization()
    .UseProtectedHangFireDashboard()
    .UseScheduledJobs()
    .UseCors();

app.MapControllers();
app.Run();
