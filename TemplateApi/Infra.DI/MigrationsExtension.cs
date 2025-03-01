using Microsoft.EntityFrameworkCore;
using Infra.Migrations.ModelDbContext;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Infra.DI
{
    public static class MigrationsExtension
    {
        public static IServiceCollection ExecuteMigrationOnStartup(this IServiceCollection services, string connectionString)
        {
            if (services is null) throw new ArgumentNullException(nameof(services));

            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException(nameof(connectionString)); 

            services.AddSingleton<MigrationsDbFactory>();

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            using (IServiceScope serviceScope = serviceProvider.CreateScope())
            {
                IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();

                MigrationsDbFactory context = serviceProvider.GetRequiredService<MigrationsDbFactory>();

                MigrationsDbContext connection = context.CreateDbContext(configuration.GetConnectionString(connectionString));

                var migrations = connection.Database.GetPendingMigrations().ToList();

                if (connection.Database.GetPendingMigrations().Any()) connection.Database.Migrate();
            }

            return services;
        }
    }
}
