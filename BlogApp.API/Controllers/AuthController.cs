using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers;
using BlogApp.Domain.Entities;
using BlogApp.Domain.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BlogApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser(RegisterDTO registerDto)
        {
            var userExists = await _userManager.FindByNameAsync(registerDto.Username);

            if (userExists != null)
            {
                return BadRequest(new { Message = "User Exists", registerDto.Username });
            }

            var user = new User()
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new { Message = "Failed to create user" });
            }

            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
            {
                var role = new IdentityRole(UserRoles.User)
                {
                    ConcurrencyStamp = Guid.NewGuid().ToString() // Ensure concurrency stamp
                };
                await _roleManager.CreateAsync(role);
            }

            await _userManager.AddToRoleAsync(user, UserRoles.User);

            return Ok(new Response(
                new { Message = "User created successfully." },
                null,
                HttpStatusCode.OK));
        }
    }

}