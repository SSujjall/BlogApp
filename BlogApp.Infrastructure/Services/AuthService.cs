using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers;
using BlogApp.Application.Interface.IRepositories;
using BlogApp.Application.Interface.IServices;
using BlogApp.Domain.Configs;
using BlogApp.Domain.Entities;
using BlogApp.Domain.Shared;
using Castle.Core.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly UserManager<Users> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWTSettings _jwtSettings;

        public AuthService(IAuthRepository authRepository, UserManager<Users> userManager,
                           RoleManager<IdentityRole> roleManager, IOptions<JWTSettings> jwtSettings)
        {
            _authRepository = authRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<ApiResponse<object>> LoginUser(LoginDTO loginDto)
        {
            var user = await _authRepository.FindByUsername(loginDto.Username);

            if (user == null)
            {
                var errors = new Dictionary<string, string> { { "Username", "Userame does not exist." } };
                return ApiResponse<object>.Failed(errors, "Login failed", HttpStatusCode.NotFound);
            }

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (isPasswordCorrect == false)
            {
                var errors = new Dictionary<string, string> { { "Password", "Invalid password" } };
                return ApiResponse<object>.Failed(errors, "Login failed", HttpStatusCode.Unauthorized);
            }

            string generatedToken = await GenerateToken(user);

            return ApiResponse<object>.Success(new { token = generatedToken }, "User Validated");
        }

        public async Task<ApiResponse<string>> RegisterUser(RegisterDTO registerDto)
        {
            // Check if the username already exists
            if (await _authRepository.UsernameExists(registerDto.Username))
            {
                var errors = new Dictionary<string, string> { { "Username", "Username already exists" } };
                return ApiResponse<string>.Failed(errors, "Register Failed.");
            }

            var user = new Users
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            // Attempt to create the user
            var createResult = await _authRepository.CreateNewUser(user, registerDto.Password);
            if (!createResult)
            {
                var errors = new Dictionary<string, string> { { "User", "Error when creating user." } };
                return ApiResponse<string>.Failed(errors, "User Creation Failed." , HttpStatusCode.InternalServerError);
            }

            // Ensure the role exists
            if (!await _roleManager.RoleExistsAsync(UserRoles.User.ToString()))
            {
                var role = new IdentityRole(UserRoles.User.ToString())
                {
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                };
                var roleCreateResult = await _roleManager.CreateAsync(role);
                if (!roleCreateResult.Succeeded)
                {
                    var errors = new Dictionary<string, string> { { "Role", "Failed to create role" } };
                    return ApiResponse<string>.Failed(errors, "Register Failed.", HttpStatusCode.InternalServerError);
                }
            }

            // Add user to the role
            var addToRoleResult = await _userManager.AddToRoleAsync(user, UserRoles.User.ToString());
            if (!addToRoleResult.Succeeded)
            {
                var errors = new Dictionary<string, string> { { "User", "Failed to assign role to user" } };
                return ApiResponse<string>.Failed(errors, "Register Failed.", HttpStatusCode.InternalServerError);
            }

            // Return success response using the correct constructor
            return ApiResponse<string>.Success(null, "User created successfully");
        }

        private async Task<string> GenerateToken(Users user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var userRole = await _authRepository.GetUserRole(user);

            var claims = new List<Claim>
            {
                new Claim("UserId", user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, userRole)
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.ValidIssuer,
                audience: _jwtSettings.ValidAudience,
                expires: DateTime.Now.AddHours(2),
                claims: claims,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
