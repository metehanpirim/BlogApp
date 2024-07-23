using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete.EfCore
{
    public class BlogContext : DbContext
    {

        public BlogContext(DbContextOptions<BlogContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<Post> Posts { get; set; }
        
        public DbSet<Tag> Tags { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<User> Users { get; set; }
    }
}