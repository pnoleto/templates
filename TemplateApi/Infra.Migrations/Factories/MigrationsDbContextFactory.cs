using Infra.Database.ModelDbContext;
using Infra.Migrations.Interfaces;
using Infra.Migrations.ModelDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infra.Migrations.Factories
{
    public class MigrationsDbContextFactory : IMigrationsDbContextFactory
    {
        public MigrationsDbContext CreateDbContext(string connectionString)
        {
            DbContextOptionsBuilder<NewsDbContext> optionsBuilder = new();

            optionsBuilder.UseSqlServer(connectionString, options => options
                    .MigrationsAssembly(typeof(MigrationsDbContext).Assembly)
                    .MigrationsHistoryTable("Migrations")
                    .EnableRetryOnFailure()
                    .MaxBatchSize(5));

            return new MigrationsDbContext(optionsBuilder.Options);
        }
    }
}
