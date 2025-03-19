using System.Net;
using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers.HelperModels;
using BlogApp.Application.Helpers.TokenHelper;
using BlogApp.Application.Interface.IRepositories;
using BlogApp.Application.Interface.IServices;
using BlogApp.Domain.Entities;
using BlogApp.Domain.Shared;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly UserManager<Users> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;

        public AuthService(IAuthRepository authRepository, UserManager<Users> userManager,
                           RoleManager<IdentityRole> roleManager, ITokenService tokenService)
        {
            _authRepository = authRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }

        public async Task<ApiResponse<RegisterResponseDTO>> RegisterUser(RegisterDTO registerDto)
        {
            // Check if the username already exists
            if (await _authRepository.UsernameExists(registerDto.Username))
            {
                var errors = new Dictionary<string, string> { { "Username", "Username already used." } };
                return ApiResponse<RegisterResponseDTO>.Failed(errors, "Register Failed.");
            }
            if (await _authRepository.EmailExists(registerDto.Email))
            {
                var errors = new Dictionary<string, string> { { "Email", "Email already used." } };
                return ApiResponse<RegisterResponseDTO>.Failed(errors, "Register Failed.");
            }

            #region request model mapping
            var user = new Users
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            #endregion

            // Attempt to create the user
            var createResult = await _authRepository.CreateNewUser(user, registerDto.Password);
            if (!createResult)
            {
                var errors = new Dictionary<string, string> { { "User", "Error when creating user." } };
                return ApiResponse<RegisterResponseDTO>.Failed(errors, "User Creation Failed.", HttpStatusCode.InternalServerError);
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

            #region Generate Email verification token
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

        public async Task<ApiResponse<LoginResponseDTO>> LoginUser(LoginDTO loginDto)
        {
            var user = await _authRepository.FindByUsername(loginDto.Username);

            if (user == null)
            {
                var errors = new Dictionary<string, string> { { "Username", "Userame does not exist." } };
                return ApiResponse<LoginResponseDTO>.Failed(errors, "Login failed", HttpStatusCode.NotFound);
            }

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (isPasswordCorrect == false)
            {
                var errors = new Dictionary<string, string> { { "Password", "Invalid password" } };
                return ApiResponse<LoginResponseDTO>.Failed(errors, "Login failed", HttpStatusCode.Unauthorized);
            }

            var isEmailVerified = await _userManager.IsEmailConfirmedAsync(user);

            if (isEmailVerified == false)
            {
                var errors = new Dictionary<string, string> { { "Email", "Email not verified" } };
                return ApiResponse<LoginResponseDTO>.Failed(errors, "Login failed", HttpStatusCode.Unauthorized);
            }

            #region generate and update jwt & refresh token with expiry in db
            string generatedToken = await _tokenService.GenerateJwtToken(user);
            string refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.Now.AddDays(7);
            await _userManager.UpdateAsync(user);
            #endregion

            #region response map
            var response = new LoginResponseDTO
            {
                JwtToken = generatedToken,
                RefreshToken = refreshToken,
            };
            #endregion

            return ApiResponse<LoginResponseDTO>.Success(response, "User Validated");
        }

        public async Task<ApiResponse<LoginResponseDTO>> RefreshToken(RefreshTokenRequestDTO model)
        {
            var principal = _tokenService.GetTokenPrincipal(model.JwtToken);
            if (principal == null)
            {
                return ApiResponse<LoginResponseDTO>.Failed(new Dictionary<string, string> { { "Token", "Invalid access token" } }, "Refresh Token Failed", HttpStatusCode.Unauthorized);
            }

            var userId = principal?.FindFirst("UserId")?.Value;
            if (userId == null)
            {
                return ApiResponse<LoginResponseDTO>.Failed(new Dictionary<string, string> { { "UserId", "Invalid user id." } }, "Refresh Token Failed", HttpStatusCode.Unauthorized);
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.RefreshToken != model.RefreshToken)
            {
                return ApiResponse<LoginResponseDTO>.Failed(new Dictionary<string, string> { { "RefreshToken", "Invalid refresh token" } }, "Refresh Failed.");
            }
            // Check if the refresh token has expired
            if (user.RefreshTokenExpiry <= DateTime.Now)
            {
                return ApiResponse<LoginResponseDTO>.Failed(new Dictionary<string, string> { { "RefreshToken", "Refresh token expired" } }, "Refresh Failed.");
            }

            #region update refresh token for user
            string newJwtToken = await _tokenService.GenerateJwtToken(user);
            string newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            /*
             * 7 days expiration for refresh token
             * Problem: Each time the jwt token is refreshed, the refresh token's expiry is set to 7 days from the refresh time.
             * If the user keeps the browser open then they will have indefinite session as tokens will keep refreshing
             * and time will be 7 days from each refresh.
             * If you dont want this then comment out the line below (RefreshTokenExpiry).
             * It will make it so that the refresh token will be updated but it's expiry will remain absolute and 
             * use will have to re authenticate after every 7 days.
             */
            user.RefreshTokenExpiry = DateTime.Now.AddDays(7);
            await _userManager.UpdateAsync(user);
            #endregion

            #region response map
            var response = new LoginResponseDTO
            {
                JwtToken = newJwtToken,
                RefreshToken = newRefreshToken
            };
            #endregion

            return ApiResponse<LoginResponseDTO>.Success(response, "Token Refreshed Successfully");
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
                // storing errors if there are any (token errors like invalid token)
                var errors = resetPasswordResult.Errors.ToDictionary(x => x.Code, x => x.Description);
                return ApiResponse<string>.Failed(errors, "User Not Found.");
            }

            return ApiResponse<string>.Success(null, "Password reset successful");
        }

        public async Task<ApiResponse<string>> LogoutUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return ApiResponse<string>.Failed(new Dictionary<string, string> { { "User", "User not found" } }, "Logout Failed.");
            }

            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;
            await _userManager.UpdateAsync(user);

            return ApiResponse<string>.Success(null, "Logged out successfully");
        }
    }
}
