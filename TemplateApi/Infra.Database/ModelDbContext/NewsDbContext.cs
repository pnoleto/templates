using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infra.Database.ModelDbContext
{
    public class NewsDbContext(DbContextOptions<NewsDbContext> options) : DbContext(options)
    {
        public DbSet<Feed> Feeds { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<Article> Articles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableDetailedErrors()
                .EnableSensitiveDataLogging();

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(NewsDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
