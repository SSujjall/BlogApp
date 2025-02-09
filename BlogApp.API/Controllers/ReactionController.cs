using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace BlogApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReactionController : ControllerBase
    {
        [HttpPost("upvote-blog")]
        public Task<IActionResult> Upvote(int blogId)
        {
            var userId = User.FindFirst("UserId").Value;

            return null;
        }
    }
}
