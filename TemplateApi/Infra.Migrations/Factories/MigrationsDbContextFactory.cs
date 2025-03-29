using Infra.Migrations.Interfaces;
using Infra.Migrations.ModelDbContext;

namespace Infra.Migrations.Factories
{
    public sealed class MigrationsDbContextFactory : IMigrationsDbContextFactory
    {
        public MigrationsDbContext CreateDbContext(string connectionString)
        {
            return new MigrationsDbContext(DbContextOptionsFactory.SqlServerDefaultOptions(connectionString).Options);
        }
    }
}
