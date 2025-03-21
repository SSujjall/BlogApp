using CloudinaryDotNet;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Infrastructure.Persistence.Health
{
    public class CloudinaryHealthCheck : IHealthCheck
    {
        private readonly Cloudinary _cloudinary;
        public CloudinaryHealthCheck(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _cloudinary.PingAsync(cancellationToken);
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return HealthCheckResult.Healthy("Cloudinary is reachable.");
                }
                else
                {
                    return HealthCheckResult.Unhealthy("Cloudinary is not reachable.");
                }
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Cloudinary is not reachable.", ex);
            }
        }
    }
}
