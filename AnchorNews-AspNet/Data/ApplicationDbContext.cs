using Microsoft.EntityFrameworkCore;

namespace AnchorNews.Data
{
    public class ApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=AnchorNewsDb;Trusted_Connection=True;");
        }

        // Add DbSet properties for your models
        // Example:
        // public DbSet<NewsPost> NewsPosts { get; set; }
    }
}