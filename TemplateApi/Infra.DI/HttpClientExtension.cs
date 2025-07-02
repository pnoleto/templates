using Microsoft.Extensions.DependencyInjection;
using Infra.Integrations;
using Refit;
using Microsoft.Extensions.Configuration;

namespace Infra.DI
{
    public static class HttpClientExtension
    {
        public static IServiceCollection AddHttpCLientFactory(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRefitClient<IHttpClient>()
            .ConfigureHttpClient(options =>
            {
                string[]? requiredHosts = configuration.GetRequiredSection("RequiredHosts").Get<string[]>();

                ArgumentNullException.ThrowIfNull(requiredHosts);

                options.DefaultRequestHeaders.Add("Content-Type", ["text/html", "text/html"]);
                options.DefaultRequestHeaders.Add("Authorization", "Bearer {MY_TOKEN}");
                options.BaseAddress = new Uri(requiredHosts[0]);
                options.Timeout = TimeSpan.FromSeconds(30);
            });

            return services;
        }
    }

}
