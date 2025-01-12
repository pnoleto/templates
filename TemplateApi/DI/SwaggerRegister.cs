using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Infra.DI
{
    public static class SwaggerRegister
    {
        public static IServiceCollection AddSwaggerDefinitions(this IServiceCollection services, IHostApplicationBuilder builder)
        {
            services.AddSwaggerGen(options =>
            {
                IApiVersionDescriptionProvider? provider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var item in provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(item.GroupName, new OpenApiInfo
                    {
                        Title = item.ApiVersion.ToString(),
                        Version = item.ApiVersion.ToString()
                    });
                    options.AddSecurityDefinition("ApiKey", new()
                    {
                        In = ParameterLocation.Header,
                        Scheme = "ApiKey",
                        Name = "X-API-KEY",
                        Type = SecuritySchemeType.ApiKey,
                        Description = "Authorization by x-api-key inside request's header"
                    });
                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                          {
                              new OpenApiSecurityScheme
                              {
                                  Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "ApiKey" }
                              },
                              Array.Empty<string>()
                          }
                     });
                }
            });
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            services.AddApiKeyAuthentication(builder.Configuration);

            return services;
        }

        public static IApplicationBuilder UseSwaggerUIDefinitions(this IApplicationBuilder builder, IServiceProvider services) =>
            builder.UseSwagger()
                .UseSwaggerUI(options =>
                {
                    var provider = services.GetRequiredService<IApiVersionDescriptionProvider>();

                    foreach (var item in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{item.GroupName}/swagger.json", item.GroupName.ToUpperInvariant());
                    }
                });
    }
}
