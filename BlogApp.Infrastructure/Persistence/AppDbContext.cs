using BlogApp.Domain.Entities;
using BlogApp.Infrastructure.Persistence.Seeders;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seeding Users
            DbSeeder.SeedUsersToDb(builder);

            // Seeding Entity Configurations
            DbSeeder.SeedConfigurationToDB(builder);
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Blogs> Blogs { get; set; } = null!;
        public DbSet<BlogHistory> BlogHistories { get; set; } = null!;
        public DbSet<Comments> Comments { get; set; } = null!;
        public DbSet<CommentHistory> CommentHistories { get; set; } = null!;
        public DbSet<BlogReaction> BlogReactions { get; set; } = null!;
        public DbSet<CommentReaction> CommentReactions { get; set; } = null!;
        public DbSet<Notifications> Notifications { get; set; } = null!;
    }
}
