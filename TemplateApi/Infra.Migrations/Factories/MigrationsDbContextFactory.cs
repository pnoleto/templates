using Infra.Migrations.ModelDbContext;
using Microsoft.EntityFrameworkCore.Design;

namespace Infra.Migrations.Factories
{
    public sealed class MigrationsDbContextFactory : IDesignTimeDbContextFactory<MigrationsDbContext>
    {
        public MigrationsDbContext CreateDbContext(string[] args)
        {
            return new MigrationsDbContext(DbContextOptionsFactory.SqlServerDefaultOptions(args[0]).Options);
        }
    }
}
