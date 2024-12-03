using BlogApp.Application.DTOs;
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
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWTSettings _jwtSettings;

        public AuthService(IAuthRepository authRepository, UserManager<User> userManager,
                           RoleManager<IdentityRole> roleManager, IOptions<JWTSettings> jwtSettings)
        {
            _authRepository = authRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<string> LoginUser(LoginDTO loginDto)
        {
            var user = await _authRepository.FindByUsername(loginDto.Username);
            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (user == null || isPasswordCorrect == false)
            {
                return null;
            }

            return await GenerateToken(user);
        }

        public async Task<string> RegisterUser(RegisterDTO registerDto)
        {
            if (await _authRepository.UsernameExists(registerDto.Username))
            {
                return "Username already taken.";
            }

            var user = new User
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var createResult = await _authRepository.CreateNewUser(user, registerDto.Password);
            if (!createResult)
            {
                return "Failed to create user";
            }

            // Ensure the role exists
            if (!await _roleManager.RoleExistsAsync(UserRoles.User.ToString()))
            {
                var role = new IdentityRole(UserRoles.User.ToString())
                {
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                };
                await _roleManager.CreateAsync(role);
            }

            await _userManager.AddToRoleAsync(user, UserRoles.User.ToString());

            return null;
        }

        private async Task<string> GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var userRole = await _authRepository.GetUserRole(user);

            var claims = new List<Claim>
            {
                new Claim("Username", user.UserName),
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
