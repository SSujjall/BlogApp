using BlogApp.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BlogApp.Infrastructure.Persistence.Health
{
    public sealed class DbHealthCheck : IHealthCheck
    {
        private readonly AppDbContext _dbContext;

        public DbHealthCheck(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                await _dbContext.Database.ExecuteSqlRawAsync("Select 1;", cancellationToken);
                return HealthCheckResult.Healthy("Database is Healthy");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Database is Unhealthy", exception: ex);
            }
        }
    }
}
