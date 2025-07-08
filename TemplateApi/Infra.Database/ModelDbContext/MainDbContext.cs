using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infra.Database.ModelDbContext
{
    public class MainDbContext(DbContextOptions<MainDbContext> options) : DbContext(options)
    {
        public DbSet<Feed> Feeds { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<Article> Articles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MainDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
