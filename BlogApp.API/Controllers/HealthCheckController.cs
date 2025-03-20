using BlogApp.Application.Helpers.HelperModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BlogApp.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        private readonly HealthCheckService _healthCheckService;

        public HealthCheckController(HealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        [HttpGet("health")]
        public async Task<IActionResult> GetHealthStatus(CancellationToken cancellationToken)
        {
            var healthReport = await _healthCheckService.CheckHealthAsync(cancellationToken);

            var response = new HealthCheckResponse
            {
                Status = healthReport.Status.ToString(),
                Message = healthReport.Status == HealthStatus.Healthy ? "Database is healthy" : "Database is unhealthy",
                Timestamp = DateTime.UtcNow
            };

            return Ok(response);
        }
    }
}
