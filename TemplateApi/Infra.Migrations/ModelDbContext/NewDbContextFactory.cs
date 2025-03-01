using Infra.Database.ModelDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infra.Migrations.ModelDbContext
{
    public class MigrationsDbContext(DbContextOptions<NewsDbContext> options) : NewsDbContext(options)
    {
    }

    public class MigrationsDbContextFactory : IDesignTimeDbContextFactory<MigrationsDbContext>
    {
        public MigrationsDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<NewsDbContext> optionsBuilder = new();

            optionsBuilder.UseSqlServer(args[0], options=> options
                    .MigrationsAssembly(typeof(MigrationsDbContext).Assembly)
                    .MigrationsHistoryTable("Migrations")
                    .EnableRetryOnFailure()
                    .MaxBatchSize(5));

            return new MigrationsDbContext(optionsBuilder.Options);
        }
    }

    public class MigrationsDbFactory
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
