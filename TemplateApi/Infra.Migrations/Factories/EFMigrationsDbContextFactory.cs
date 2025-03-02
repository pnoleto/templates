using Infra.Database.ModelDbContext;
using Infra.Migrations.ModelDbContext;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Infra.Migrations.Factories
{
    public class EFMigrationsDbContextFactory : IDesignTimeDbContextFactory<MigrationsDbContext>
    {
        public MigrationsDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<NewsDbContext> optionsBuilder = new();

            optionsBuilder.UseSqlServer(args[0], options => options
                    .MigrationsAssembly(typeof(MigrationsDbContext).Assembly)
                    .MigrationsHistoryTable("Migrations")
                    .EnableRetryOnFailure()
                    .MaxBatchSize(5));

            return new MigrationsDbContext(optionsBuilder.Options);
        }
    }
}
