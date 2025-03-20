using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace BlogApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [EnableRateLimiting("FixedPolicy")]
        [HttpGet("check")]
        public IActionResult Get()
        {
            return Ok("API is working.");
        }
    }
}
