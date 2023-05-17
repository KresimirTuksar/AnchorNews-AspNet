﻿using AnchorNews_AspNet.Models.NewsPost;
using AnchorNews_AspNet.Models.UserAuth;
using Microsoft.EntityFrameworkCore;

namespace AnchorNews.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> NewsPosts { get; set; }
        //public DbSet<Comment> Comments { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=AnchorNewsDb;Trusted_Connection=True;");
        }

    }
}