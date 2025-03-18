using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Infra.DI
{
    public static class SwaggerExtension
    {
        private static SwaggerGenOptions LoadXmlDocument(this SwaggerGenOptions options, string? xmlDocumentName)
        {
            string xmlExtension = ".xml";

            string xmlPath = $"{new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName}/{xmlDocumentName}{xmlExtension}";

            options.IncludeXmlComments(xmlPath);

            return options;
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

        private static IServiceCollection AddSwaggerApiVersionsGenerator(this IServiceCollection services, string? xmlDocumentName)
        {
            return services.AddSwaggerGen(options =>
            {
                IApiVersionDescriptionProvider? provider = services.BuildServiceProvider()
                .GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (ApiVersionDescription apiVersion in provider.ApiVersionDescriptions)
                {
                    options.AddSwaggerDocument(apiVersion);
                }

                if (!string.IsNullOrEmpty(xmlDocumentName))
                    options.LoadXmlDocument(xmlDocumentName);
            });
        }

        public static IServiceCollection AddSwaggerDefinitions(this IServiceCollection services, string? xmlDocumentName)
        {
            return services.AddSwaggerApiVersionsGenerator(xmlDocumentName)
                .AddApiExplorerWithVersioning();
        }

        public static IApplicationBuilder UseSwaggerDefinitions(this IApplicationBuilder builder) =>
            builder.UseSwagger()
                .UseSwaggerUI(options =>
                {
                    IApiVersionDescriptionProvider provider = builder.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

                    foreach (ApiVersionDescription item in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{item.GroupName}/swagger.json", item.GroupName.ToUpperInvariant());
                    }
                });
    }
}
