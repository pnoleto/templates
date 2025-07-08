using Infra.Database.ModelDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infra.Migrations.ModelDbContext
{
    public class MigrationsDbContext(DbContextOptions<MainDbContext> options) : MainDbContext(options)
    {
    }
}
