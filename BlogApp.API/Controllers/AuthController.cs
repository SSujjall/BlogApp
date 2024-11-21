using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers;
using BlogApp.Application.Interface.IServices;
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
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController(IAuthService authService, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _authService = authService;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser(RegisterDTO registerDto)
        {
            var result = await _authService.RegisterUser(registerDto);

            if (result != null)
            {
                return BadRequest(new Response(
                    null,
                    new Dictionary<string, string>
                    {
                        { "message", "Username already exists." }
                    },
                    HttpStatusCode.BadRequest));
            }

            return Ok(new Response(
                new { Message = "User created successfully." },
                null,
                HttpStatusCode.OK));
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser(LoginDTO model)
        {
            var token = await _authService.LoginUser(model);

            if (token == null)
            {
                return BadRequest(new Response(
                    null,
                    new Dictionary<string, string>
                    {
                        { "message", "Invalid username or password." }
                    },
                    HttpStatusCode.BadRequest));
            }

            return Ok(new Response(
                new
                {
                    Message = "User validated successfully",
                    Token = token
                },
                null,
                HttpStatusCode.OK));
        }
    }
}