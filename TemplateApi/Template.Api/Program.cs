using Infra.DI;
using Shared.Mediator;
using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddConfigurationItems();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer()
     //.ExecuteMigrationsOnStartup(builder.Configuration, "NewsConnection")
     .LoadAllMediatorWorkflows(Assembly.GetAssembly(typeof(Mediator)))
     .AddHttpCLientFactory(builder.Configuration)
     .AddInMemoryDbContext()
     .AddExceptionHandler()
     .AddCorsDefinitions()
     .AddRepositories()
     .AddHttpClient()
     .AddLogging()
     .AddOpenApi("v1")
     .AddOpenApi("v2");

builder.Services
    .AddHealthChecks()
    .CheckSystem()
    .CheckPostgreSql(builder.Configuration, "NewsConnection");

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
