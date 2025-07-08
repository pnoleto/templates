using Infra.Database.ModelDbContext;
using Infra.Migrations.ModelDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infra.Migrations.Factories
{
    internal static class DbContextOptionsFactory
    {
        private const int MaxRetryCount = 3;
        private const int MaxBatchSize = 5;

        public static DbContextOptionsBuilder<MainDbContext> SqlServerDefaultOptions(string connectionString)
        {
            return new DbContextOptionsBuilder<MainDbContext>()
                    .UseSqlServer(connectionString, options => options
                    .MigrationsAssembly(typeof(MigrationsDbContext).Assembly)
                    .MigrationsHistoryTable("Migrations")
                    .EnableRetryOnFailure(MaxRetryCount)
                    .MaxBatchSize(MaxBatchSize));
        }
    }
}
