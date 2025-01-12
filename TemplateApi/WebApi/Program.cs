using Infra.DI;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer()
    .AddSwaggerDefinitions(builder)
    .AddInMemoryDbContext()
    .AddRepositories()
    .AddFeedRobots()
    .AddRangFireSchedulerWithInMemoryDb()
    .AddCorsDefinitions()
    .AddLogging()
    .AddMediator();

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
