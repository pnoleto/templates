using Infra.Database.ModelDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infra.Migrations.ModelDbContext
{
    public class MigrationsDbContext(DbContextOptions<NewsDbContext> options) : NewsDbContext(options)
    {
    }
}
