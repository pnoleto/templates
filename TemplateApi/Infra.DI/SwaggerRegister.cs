using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Infra.DI
{
    public static class SwaggerRegister
    {
        public static IServiceCollection AddSwaggerDefinitions(this IServiceCollection services)
        {
            return services.AddSwaggerApiVersionsGeneratorWithApiKeyDefinitions()
                .AddApiExplorerWithVersioning()
                .AddApiKeyAuthentication();
        }

        private static IServiceCollection AddSwaggerApiVersionsGeneratorWithApiKeyDefinitions(this IServiceCollection services)
        {
            return services.AddSwaggerGen(options =>
            {
                IApiVersionDescriptionProvider? provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (ApiVersionDescription apiVersion in provider.ApiVersionDescriptions)
                {
                    options.AddSwaggerDocument(apiVersion)
                        .AddApiKeySecurityDefinition()
                        .AddApiKeySecurityRequirement();
                }
            });
        }

        private static SwaggerGenOptions AddSwaggerDocument(this SwaggerGenOptions options, ApiVersionDescription item)
        {
            options.SwaggerDoc(item.GroupName, new OpenApiInfo
            {
                Title = item.ApiVersion.ToString(),
                Version = item.ApiVersion.ToString()
            });

            return options;
        }

        private static SwaggerGenOptions AddApiKeySecurityRequirement(this SwaggerGenOptions options)
        {
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                       Reference = new OpenApiReference
                       {
                           Id = "ApiKey",
                           Type = ReferenceType.SecurityScheme
                       }
                    },
                    Array.Empty<string>()
                }
            });

            return options;
        }

        private static SwaggerGenOptions AddApiKeySecurityDefinition(this SwaggerGenOptions options)
        {
            options.AddSecurityDefinition("ApiKey", new()
            {
                In = ParameterLocation.Header,
                Scheme = "ApiKey",
                Name = "x-api-key",
                Type = SecuritySchemeType.ApiKey,
                Description = "Authorization by x-api-key inside request's header"
            });

            return options;
        }

        private static IServiceCollection AddApiExplorerWithVersioning(this IServiceCollection services)
        {
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

            return services;
        }

        public static IApplicationBuilder UseSwaggerUIDefinitions(this IApplicationBuilder builder) =>
            builder.UseSwagger()
                .UseSwaggerUI(options =>
                {
                    var provider = builder.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

                    foreach (var item in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{item.GroupName}/swagger.json", item.GroupName.ToUpperInvariant());
                    }
                });
    }
}
