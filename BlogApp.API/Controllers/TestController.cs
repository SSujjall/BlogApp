using BlogApp.Application.DTOs;
using BlogApp.Application.Interface.IServices;
using BlogApp.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace BlogApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IPlaygroundService _service;

        public TestController()
        {
            _service = new PlaygroundService();
        }

        [EnableRateLimiting("ReadPolicy")]
        [HttpGet("check")]
        public IActionResult Get()
        {
            return Ok("API is working.");
        }

        [HttpPost("PrintData")]
        public IActionResult TestPostPrintData([FromBody] PlaygroundDTO model)
        {
            _service.ChangeData(model);
            return Ok(_service.PrintData());
        }
    }
}
