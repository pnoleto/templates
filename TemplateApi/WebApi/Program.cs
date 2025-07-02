using Infra.DI;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddConfigurationItems();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer()
     .ExecuteMigrationsOnStartup(builder.Configuration, "NewsConnection")
     .AddHttpCLientFactory(builder.Configuration)
     .AddExceptionHandler()
     .AddCorsDefinitions()
     .AddRepositories()
     .AddHttpClient()
     .AddLogging()
     .AddOpenApi();

builder.Services
    .AddHealthChecks()
    .CheckSystem()
    .CheckSqlServer(builder.Configuration, "NewsConnection");

WebApplication app = builder.Build();

app.UseRouting()
    .UseAuthentication()
    .UseAuthorization()
    .UseExceptionHandler()
    .UseHttpsRedirection()
    .UseCors()
    .UseHsts();

app.UseHealthCheckEndpoint();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

await app.RunAsync();
