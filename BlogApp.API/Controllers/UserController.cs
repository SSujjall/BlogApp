using System.Net;
using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers.CloudinaryService.Service;
using BlogApp.Application.Interface.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace BlogApp.API.Controllers
{
    [EnableRateLimiting("ReadPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService _userService, ICloudinaryService _cloudinary) : ControllerBase
    {
        [Authorize]
        [HttpGet("who-am-i")]
        public async Task<IActionResult> GetUserByToken()
        {
            var userId = User.FindFirst("UserId")?.Value;

            var response = await _userService.GetUserById(userId);
            if (response.Status != true)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Superadmin,admin")]
        [HttpGet("get-all")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            var response = await _userService.GetAllUsers();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var response = await _userService.GetUserById(userId);
            if (response.Status != true)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpPatch("change-pw")]
        public async Task<IActionResult> ChangeUserPassword(UpdateUserPwDTO model)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authorized.");
            }

            model.UserId = userId;
            var response = await _userService.ChangeUserPassword(model);
            if (response.Status != true)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpPut("update-details")]
        public async Task<IActionResult> UpdateUserDetail(UserProfileReqDTO model)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authorized.");
            }

            model.UserId = userId;
            var response = await _userService.UpdateUserDetail(model);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Test()
        {
            return Ok();
        }
    }
}
