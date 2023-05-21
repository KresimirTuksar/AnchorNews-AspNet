using AnchorNews_AspNet.Models.ApiNewsPost;
using AnchorNews_AspNet.Models.Comments;
using AnchorNews_AspNet.Models.NewsPost;
using AnchorNews_AspNet.Models.UserAuth;
using Microsoft.EntityFrameworkCore;

namespace AnchorNews.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Post> NewsPosts { get; set; }
        public DbSet<ApiPost> ApiNewsPosts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=AnchorNewsDb;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.NewsPost)
                .WithMany(np => np.Comments)
                .HasForeignKey(c => c.NewsPostId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}