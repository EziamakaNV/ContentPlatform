using Microsoft.EntityFrameworkCore;
using Newsletter.Reporting.Entities;

namespace Newsletter.Reporting.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleEvent> ArticleEvents { get; set; }
    }
}
