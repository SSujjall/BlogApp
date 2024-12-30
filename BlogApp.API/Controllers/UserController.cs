using BlogApp.Application.Interface.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService _userService) : ControllerBase
    {
        [Authorize]
        [HttpGet("get-user-from-token")]
        public async Task<IActionResult> GetUserById()
        {
            var userId = User.FindFirst("UserId")?.Value;

            var response = await _userService.GetUserById(userId);
            if (response.Status != true)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }
    }
}
