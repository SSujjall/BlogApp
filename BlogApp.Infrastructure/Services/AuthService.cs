using System.Net;
using BlogApp.Application.DTOs;
using BlogApp.Application.Exceptions;
using BlogApp.Application.Helpers.HelperModels;
using BlogApp.Application.Helpers.TokenHelper;
using BlogApp.Application.Interface.IServices;
using BlogApp.Domain.Entities;
using BlogApp.Domain.Shared;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<Users> _userManager;
        private readonly ITokenService _tokenService;
        private readonly ITransactionService _transactionService;

        public AuthService(
            UserManager<Users> userManager,
            ITokenService tokenService,
            ITransactionService transactionService
        )
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _transactionService = transactionService;
        }

        public async Task<ApiResponse<RegisterResponseDTO>> RegisterUser(RegisterDTO registerDto)
        {
            return await _transactionService.ExecuteInTransactionAsync(async () =>
            {
                #region request model mapping
                var user = new Users
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email,
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                #endregion

                // Attempt to create the user
                var userCreateResult = await _userManager.CreateAsync(user, registerDto.Password);
                if (!userCreateResult.Succeeded)
                {
                    var errors = userCreateResult.Errors.ToDictionary(
                        e => e.Code,
                        e => e.Description
                    );
                    throw new ServiceException(errors, HttpStatusCode.Conflict);
                }

                // Add user to the 'User' role
                var addToRoleResult = await _userManager.AddToRoleAsync(user, UserRoles.User.ToString());
                if (!addToRoleResult.Succeeded)
                {
                    var errors = addToRoleResult.Errors.ToDictionary(
                        e => e.Code,
                        e => e.Description
                    );
                    throw new ServiceException(errors, HttpStatusCode.Conflict);
                }

                #region Generate Email verification token
                var emailVerificationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var response = new RegisterResponseDTO
                {
                    EmailConfirmToken = emailVerificationToken
                };
                #endregion

                return ApiResponse<RegisterResponseDTO>.Success(response, "User created successfully");
            });
        }

        public async Task<string> GenerateEmailVerificationToken(ResendVerificationDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null || user.EmailConfirmed)
            {
                return null;
            }

            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<ApiResponse<LoginResponseDTO>> LoginUser(LoginDTO loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);
            if (user == null)
            {
                return InvalidLoginResponse();
            }
            if (user.EmailConfirmed == false)
            {
                var errors = new Dictionary<string, string> { { "UnverifiedEmail", "Email is not verified. Please verify email first and try again" } };
                return ApiResponse<LoginResponseDTO>.Failed(errors, "Login failed", HttpStatusCode.Unauthorized);
            }

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (isPasswordCorrect == false)
            {
                return InvalidLoginResponse();
            }

            #region generate and update jwt & refresh token with expiry in db
            string generatedToken = await _tokenService.GenerateJwtToken(user);
            string refreshToken = await _tokenService.GenerateRefreshToken();

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

        private ApiResponse<LoginResponseDTO> InvalidLoginResponse()
        {
            var errors = new Dictionary<string, string>
            {
                { "InvalidCredentials", "Invalid username or password." }
            };

            return ApiResponse<LoginResponseDTO>.Failed(errors, "Login failed", HttpStatusCode.Unauthorized);
        }

        public async Task<ApiResponse<LoginResponseDTO>> RefreshToken(RefreshTokenRequestDTO model)
        {
            var principal = _tokenService.GetTokenPrincipal(model.JwtToken);
            if (principal == null)
            {
                return ApiResponse<LoginResponseDTO>.Failed(new Dictionary<string, string> { { "Token", "Invalid access token or user id" } }, "Refresh Token Failed", HttpStatusCode.Unauthorized);
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
            string newRefreshToken = await _tokenService.GenerateRefreshToken();

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

            if (user.RefreshToken == null || user.RefreshTokenExpiry == null)
            {
                return ApiResponse<string>.Failed(new Dictionary<string, string> { { "User", "User already logged out" } }, "Logout Failed.");
            }

            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;
            await _userManager.UpdateAsync(user);

            return ApiResponse<string>.Success(null, "Logged out successfully");
        }
    }
}
