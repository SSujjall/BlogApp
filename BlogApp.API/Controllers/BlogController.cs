using BlogApp.Application.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BlogApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllBlogs()
        {
            return Ok(new Response(
                new { Message = "blogs" },
                null,
                HttpStatusCode.OK)
            );
        }

        [AllowAnonymous]
        [HttpGet("get-blog/{id}")]
        public Task<IActionResult> GetBlog()
        {
            return null;
        }

        [Authorize]
        [HttpPost("create")]
        public Task<IActionResult> CreateBlog()
        {
            return null;
        }

        [Authorize]
        [HttpPost("update")]
        public Task<IActionResult> UpdateBlog()
        {
            return null;
        }

        [Authorize]
        [HttpPost("delete/{id}")]
        public Task<IActionResult> DeleteBlog(int id)
        {
            return null;
        }
    }
}
