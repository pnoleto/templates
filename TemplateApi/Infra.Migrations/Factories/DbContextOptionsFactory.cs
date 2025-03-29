using Infra.Database.ModelDbContext;
using Infra.Migrations.ModelDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infra.Migrations.Factories
{
    internal static class DbContextOptionsFactory
    {
        public static DbContextOptionsBuilder<NewsDbContext> SqlServerDefaultOptions(string connectionString)
        {
            ArgumentNullException.ThrowIfNull(nameof(connectionString));

            return new DbContextOptionsBuilder<NewsDbContext>()
                    .UseSqlServer(connectionString, options => options
                    .MigrationsAssembly(typeof(MigrationsDbContext).Assembly)
                    .MigrationsHistoryTable("Migrations")
                    .EnableRetryOnFailure(3)
                    .MaxBatchSize(5));
        }
    }
}
