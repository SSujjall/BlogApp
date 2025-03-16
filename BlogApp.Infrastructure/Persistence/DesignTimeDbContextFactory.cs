using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BlogApp.Infrastructure.Persistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // Set up configuration to read the connection string from environment variables
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Adjust the base path if needed
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables() // This will allow reading from environment variables
                .Build();

            // Retrieve the connection string from environment variables or appsettings.json
            var connectionString = configuration.GetConnectionString("BlogDB")
                                 ?? Environment.GetEnvironmentVariable("ConnectionStrings__BlogDB");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'BlogDB' not found.");
            }

            optionsBuilder.UseSqlServer(connectionString); // Or use the appropriate provider

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
