using Microsoft.EntityFrameworkCore;
using Infra.Migrations.ModelDbContext;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Infra.Migrations.Interfaces;
using Infra.Migrations.Factories;
using Microsoft.Extensions.Logging;

namespace Infra.DI
{
    public static class MigrationsExtension
    {
        public static IServiceCollection ExecuteMigrationsOnStartup(this IServiceCollection services, IConfiguration configuration, string connectionStringName)
        {
            ArgumentNullException.ThrowIfNull(services);

            ArgumentNullException.ThrowIfNull(connectionStringName); 

            services.AddSingleton<IMigrationsDbContextFactory, MigrationsDbContextFactory>();

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            ILogger logger = serviceProvider.GetRequiredService<ILogger<MigrationsDbContextFactory>>();

            using (IServiceScope serviceScope = serviceProvider.CreateScope())
            {
                string? connectionString = string.Empty;

                logger.LogInformation("Getting ConnectionString...");

                connectionString = configuration.GetConnectionString(connectionStringName);

                logger.LogInformation("ConnectionString succesful read...");

                ArgumentNullException.ThrowIfNull(connectionString);

                IMigrationsDbContextFactory context = serviceProvider.GetRequiredService<IMigrationsDbContextFactory>();

                logger.LogInformation("Creating connection...");

                MigrationsDbContext connection = context.CreateDbContext(connectionString);

                logger.LogInformation("Connection succesful created...");

                logger.LogInformation("Fiding pending migrations...");

                IList<string> migrations = [..connection.Database.GetPendingMigrations()];

                logger.LogInformation("Succesful found pending migrations...count: {0}", migrations.Count);

                if (migrations.Any()) connection.Database.Migrate();

                logger.LogInformation("Migrations applied succesful...");
            }

            return services;
        }
    }
}
