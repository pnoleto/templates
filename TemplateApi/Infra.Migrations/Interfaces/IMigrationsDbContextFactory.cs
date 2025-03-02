using Infra.Migrations.ModelDbContext;

namespace Infra.Migrations.Interfaces
{
    public interface IMigrationsDbContextFactory
    {
        MigrationsDbContext CreateDbContext(string connectionString);
    }
}
