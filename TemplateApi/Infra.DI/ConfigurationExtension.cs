using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Settings;

namespace Infra.DI
{
    public static class ConfigurationExtension
    {
        private static T LoadConfig<T>(this IConfiguration configuration, string SectionName) where T : SettingsBase
        {
            T? instance = configuration.GetRequiredSection(SectionName).Get<T>();

            return instance?? throw new ArgumentNullException(SectionName);
        }

        public static IHostApplicationBuilder AddConfigurationItems(this IHostApplicationBuilder builder)
        {
            builder.Services.AddSingleton(config => builder.Configuration.LoadConfig<CorsSettings>("Cors"));

            return builder;
        }
    }
}
