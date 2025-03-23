using Microsoft.Extensions.DependencyInjection;
using Infra.Integrations;
using Refit;

namespace Infra.DI
{
    public static class HttpClientExtension
    {
        public static IServiceCollection AddHttpCLientFactory(this IServiceCollection services)
        {
            services.AddRefitClient<IGoogleClient>()
            .ConfigureHttpClient(options =>
            {
                options.DefaultRequestHeaders.Add("Content-Type", ["text/html", "text/html"]);
                options.DefaultRequestHeaders.Add("Authorization", "Bearer {MY_TOKEN}");
                options.BaseAddress = new Uri("https://www.google.com");
                options.Timeout = TimeSpan.FromSeconds(30);
            });

            return services;
        }
    }

}
