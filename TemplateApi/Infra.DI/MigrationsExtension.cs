using Microsoft.EntityFrameworkCore;
using Infra.Migrations.ModelDbContext;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Infra.Migrations.Factories;
using Microsoft.EntityFrameworkCore.Design;

namespace Infra.DI
{
    public static class MigrationsExtension
    {
        public static IServiceCollection ExecuteMigrationsOnStartup(this IServiceCollection services, IConfiguration configuration, string connectionStringName)
        {
            ArgumentNullException.ThrowIfNull(services);

            ArgumentNullException.ThrowIfNull(connectionStringName);

            services.AddSingleton<IDesignTimeDbContextFactory<MigrationsDbContext>, MigrationsDbContextFactory>();

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            using IServiceScope serviceScope = serviceProvider.CreateScope();

            string? connectionString = string.Empty;

            connectionString = configuration.GetConnectionString(connectionStringName);

            ArgumentNullException.ThrowIfNull(connectionString);

            IDesignTimeDbContextFactory<MigrationsDbContext> context = serviceProvider.GetRequiredService<IDesignTimeDbContextFactory<MigrationsDbContext>>();

            MigrationsDbContext connection = context.CreateDbContext([connectionString]);

            IList<string> migrations = [..connection.Database.GetPendingMigrations()];

            if (migrations.Any()) connection.Database.Migrate();

            return services;
        }
    }
}
