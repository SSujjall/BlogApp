using BlogApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

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

            var adminUserId = Guid.NewGuid().ToString();
            var adminRoleId = Guid.NewGuid().ToString();

            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Name = "Admin",
                NormalizedName = "ADMIN",
                Id = adminRoleId,
                ConcurrencyStamp = adminRoleId
            });

            var adminUser = new User()
            {
                Id = adminUserId,
                Email = "admin@user.com",
                UserName = "admin@user.com",
                NormalizedEmail = "ADMIN@USER.COM",
                NormalizedUserName = "ADMIN@USER.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
            };

            var passwordHash = new PasswordHasher<User>();
            const string password = "Admin@123";

            adminUser.PasswordHash = passwordHash.HashPassword(adminUser, password);

            builder.Entity<User>().HasData(adminUser);

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = adminRoleId,
                UserId = adminUserId
            });

            // Blog configuration
            builder.Entity<Blogs>()
                .HasOne(b => b.User)
                .WithMany(u => u.Blogs)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // BlogHistory configuration
            builder.Entity<BlogHistory>()
                .HasOne(bh => bh.Blogs)
                .WithMany(b => b.History)
                .HasForeignKey(bh => bh.BlogId)
                .OnDelete(DeleteBehavior.Cascade);

            // Comment configuration
            builder.Entity<Comments>()
                .HasOne(c => c.Blogs)
                .WithMany(b => b.Comments)
                .HasForeignKey(c => c.BlogId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Comments>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Reaction configurations
            builder.Entity<BlogReaction>()
                .HasOne(br => br.Blogs)
                .WithMany(b => b.Reactions)
                .HasForeignKey(br => br.BlogId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<BlogReaction>()
                .HasOne(br => br.User)
                .WithMany(u => u.BlogReactions)
                .HasForeignKey(br => br.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Prevent duplicate reactions
            builder.Entity<BlogReaction>()
                .HasIndex(br => new { br.BlogId, br.UserId })
                .IsUnique();

            builder.Entity<CommentReaction>()
                .HasIndex(cr => new { cr.CommentId, cr.UserId })
                .IsUnique();

            // CommentHistory configuration
            builder.Entity<CommentHistory>()
                .HasOne(ch => ch.Comments)
                .WithMany(c => c.History)
                .HasForeignKey(ch => ch.CommentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CommentHistory>()
                .HasOne(ch => ch.ModifiedBy)
                .WithMany()
                .HasForeignKey(ch => ch.ModifiedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Blogs> Blogs { get; set; } = null!;
        public DbSet<BlogHistory> BlogHistories { get; set; } = null!;
        public DbSet<Comments> Comments { get; set; } = null!;
        public DbSet<CommentHistory> CommentHistories { get; set; } = null!;
        public DbSet<BlogReaction> BlogReactions { get; set; } = null!;
        public DbSet<CommentReaction> CommentReactions { get; set; } = null!;
        public DbSet<Notifications> Notifications { get; set; } = null!;
    }
}
