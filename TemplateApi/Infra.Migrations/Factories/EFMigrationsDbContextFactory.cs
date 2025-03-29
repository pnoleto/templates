using Infra.Migrations.ModelDbContext;
using Microsoft.EntityFrameworkCore.Design;

namespace Infra.Migrations.Factories
{
    internal sealed class EFMigrationsDbContextFactory : IDesignTimeDbContextFactory<MigrationsDbContext>
    {
        public MigrationsDbContext CreateDbContext(string[] args)
        {
            return new MigrationsDbContext(DbContextOptionsFactory.SqlServerDefaultOptions(args[0]).Options);
        }
    }
}
