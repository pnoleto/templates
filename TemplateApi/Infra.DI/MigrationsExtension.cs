using Microsoft.EntityFrameworkCore;
using Infra.Migrations.ModelDbContext;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Infra.Migrations.Interfaces;
using Infra.Migrations.Factories;

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

            using (IServiceScope serviceScope = serviceProvider.CreateScope())
            {
                string? connectionString = string.Empty;
               
                connectionString = configuration.GetConnectionString(connectionStringName);

                ArgumentNullException.ThrowIfNull(connectionString);

                IMigrationsDbContextFactory context = serviceProvider.GetRequiredService<IMigrationsDbContextFactory>();

                MigrationsDbContext connection = context.CreateDbContext(connectionString);

                IList<string> migrations = [..connection.Database.GetPendingMigrations()];

                if (connection.Database.GetPendingMigrations().Any()) connection.Database.Migrate();
            }

            return services;
        }
    }
}
