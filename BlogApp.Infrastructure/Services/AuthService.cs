using Azure;
using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers.HelperModels;
using BlogApp.Application.Interface.IRepositories;
using BlogApp.Application.Interface.IServices;
using BlogApp.Domain.Configs;
using BlogApp.Domain.Entities;
using BlogApp.Domain.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace BlogApp.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly UserManager<Users> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtConfig _jwtSettings;

        public AuthService(IAuthRepository authRepository, UserManager<Users> userManager,
                           RoleManager<IdentityRole> roleManager, IOptions<JwtConfig> jwtSettings)
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

            var isEmailVerified = await _userManager.IsEmailConfirmedAsync(user);

            if (isEmailVerified == false)
            {
                var errors = new Dictionary<string, string> { { "Email", "Email not verified" } };
                return ApiResponse<object>.Failed(errors, "Login failed", HttpStatusCode.Unauthorized);
            }

            string generatedToken = await GenerateToken(user);

            return ApiResponse<object>.Success(new { token = generatedToken }, "User Validated");
        }

        public async Task<ApiResponse<RegisterResponseDTO>> RegisterUser(RegisterDTO registerDto)
        {
            // Check if the username already exists
            if (await _authRepository.UsernameExists(registerDto.Username))
            {
                var errors = new Dictionary<string, string> { { "Username", "Username already exists" } };
                return ApiResponse<RegisterResponseDTO>.Failed(errors, "Register Failed.");
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
                return ApiResponse<RegisterResponseDTO>.Failed(errors, "User Creation Failed." , HttpStatusCode.InternalServerError);
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
                    return ApiResponse<RegisterResponseDTO>.Failed(errors, "Register Failed.", HttpStatusCode.InternalServerError);
                }
            }

            // Add user to the role
            var addToRoleResult = await _userManager.AddToRoleAsync(user, UserRoles.User.ToString());
            if (!addToRoleResult.Succeeded)
            {
                var errors = new Dictionary<string, string> { { "User", "Failed to assign role to user" } };
                return ApiResponse<RegisterResponseDTO>.Failed(errors, "Register Failed.", HttpStatusCode.InternalServerError);
            }

            #region Generate email verification token
            var existingUser = await _authRepository.FindByUsername(registerDto.Username);
            if (existingUser == null)
            {
                var errors = new Dictionary<string, string> { { "User", "Failed to fetch newly created user." } };
                return ApiResponse<RegisterResponseDTO>.Failed(errors, "Failed to fetch user.");
            }
            var emailVerificationToken = await _userManager.GenerateEmailConfirmationTokenAsync(existingUser);
            var response = new RegisterResponseDTO
            {
                EmailConfirmToken = emailVerificationToken
            };
            #endregion

            return ApiResponse<RegisterResponseDTO>.Success(response, "User created successfully");
        }

        public async Task<ApiResponse<string>> ConfirmEmailVerification(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                var errors = new Dictionary<string, string> { { "User", "Incorrect Email, user not found." } };
                return ApiResponse<string>.Failed(errors, "User Not Found.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                var errors = new Dictionary<string, string> { { "Email Confirmation", "Failed to confirm Email, Invalid Token." } };
                return ApiResponse<string>.Failed(errors, "Email Confirmation Failed.");
            }
            return ApiResponse<string>.Success(null, "Email Confirmed successfully");
        }

        public async Task<ApiResponse<ForgotPasswordResponseDTO>> GenerateForgotPasswordLink(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                var errors = new Dictionary<string, string> { { "User", "Incorrect Email, user not found." } };
                return ApiResponse<ForgotPasswordResponseDTO>.Failed(errors, "User Not Found.");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (token == null)
            {
                var errors = new Dictionary<string, string> { { "Token", "Error when generating forgot password token." } };
                return ApiResponse<ForgotPasswordResponseDTO>.Failed(errors, "Token Generation Failed");
            }

            var response = new ForgotPasswordResponseDTO
            {
                ForgotPasswordToken = token
            };
            return ApiResponse<ForgotPasswordResponseDTO>.Success(response, "Forgot Password Token Generated successfully");
        }

        public async Task<ApiResponse<string>> ResetPasswordAsync(ResetPasswordDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                var errors = new Dictionary<string, string> { { "User", "Incorrect Email, user not found." } };
                return ApiResponse<string>.Failed(errors, "User Not Found.");
            }

            var resetPasswordResult = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (!resetPasswordResult.Succeeded)
            {
                // storing errors if there are any
                var errors = resetPasswordResult.Errors.Select(x => x.Description).ToList();
                return ApiResponse<string>.Failed(null, "User Not Found.");
            }

            return ApiResponse<string>.Success(null, "Password reset successful");
        }

        #region helper methods
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
        #endregion
    }
}
