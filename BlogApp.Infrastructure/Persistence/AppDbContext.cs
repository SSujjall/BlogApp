using BlogApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = adminRoleId,
                UserId = adminUserId
            });

            var passwordHash = new PasswordHasher<User>();
            const string password = "Admin@123";

            adminUser.PasswordHash = passwordHash.HashPassword(adminUser, password);

            builder.Entity<User>().HasData(adminUser);
        }

        public DbSet<User> Users { get; set; }
    }
}
