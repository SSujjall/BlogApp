using Azure;
using BlogApp.Application.DTOs;
using BlogApp.Application.Interface.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogReactionController : ControllerBase
    {
        private readonly IBlogReactionService _blogReactionService;
        public BlogReactionController(IBlogReactionService blogReactionService)
        {
            _blogReactionService = blogReactionService;
        }

        [HttpGet("get-all/{blogId}")]
        public async Task<IActionResult> GetAllReactions(int blogId)
        {
            var response = await _blogReactionService.GetAllBlogVotes(blogId);
            if (response.Status == false)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpPost("vote")]
        public async Task<IActionResult> VoteBlog(AddOrUpdateBlogReactionDTO model)
        {
            var userId = User?.FindFirst("UserId")?.Value;
            if (userId == null)
            {
                return Unauthorized("User invalid");
            }

            var response = await _blogReactionService.VoteBlog(model, userId);
            if (response.Status == false)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }
    }
}
