using Infra.Integrations;
using Microsoft.Extensions.DependencyInjection;
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
                options.BaseAddress = new Uri("https://www.google.com");
                options.Timeout = TimeSpan.FromSeconds(30);
            });

            return services;
        }
    }
}
