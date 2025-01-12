using Infra.DI;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddOpenTelemetryLogger();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer()
     .AddOpenTelemetryInstrumentation(builder.Configuration)
     .AddRangFireSchedulerWithInMemoryDb()
     .AddSwaggerDefinitions(builder)
     .AddInMemoryDbContext()
     .AddCorsDefinitions()
     .AddRepositories()
     .AddFeedRobots()
     .AddMediator()
     .AddLogging();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUIDefinitions(app.Services);
}

app.UseProtectedHangFireDashboard()
    .UseHttpsRedirection()
    .UseAuthentication()
    .UseAuthorization()
    .UseScheduledJobs()
    .UseCors();

app.MapControllers();
app.Run();
