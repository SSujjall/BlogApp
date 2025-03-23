using System.Net;
using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers.GoogleAuthService.Config;
using BlogApp.Application.Helpers.GoogleAuthService.Model;
using BlogApp.Application.Helpers.HelperModels;
using BlogApp.Application.Helpers.TokenHelper;
using BlogApp.Domain.Entities;
using BlogApp.Domain.Shared;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace BlogApp.Application.Helpers.GoogleAuthService.Service
{
    #region Get Testing Token for Google Auth
    /// Notes to remember:
    /// in oauth credentials in Google Cloud Console, the redirect URI must have
    /// localhost with port url and
    /// https://developers.google.com/oauthplayground for using google playground for creating testing tokens
    /// and the url of the production if deployed. (right now its in vercel)

    // 1. Get a Google ID Token for Testing
    // You need a valid Google ID token to test your API.
    // Get a Token from Google OAuth Playground
    // - Go to Google OAuth Playground
    // - Click on “Configuration” (top-right gear icon)
    // - Check "Use your own OAuth credentials" and enter:
    // - Client ID: Your Google OAuth Client ID(from GoogleConfig in your app settings)
    // - Client Secret: Your Google OAuth Client Secret
    // - Select Google Sign-In scopes:
    //   - https://www.googleapis.com/auth/userinfo.email
    // - Click "Authorize APIs" → Click "Exchange authorization code for tokens"
    // - Copy the token_id from the response
    #endregion
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly GoogleConfig _googleConfig;
        private readonly UserManager<Users> _userManager;
        private readonly ITokenService _tokenService;

        public GoogleAuthService(IOptions<GoogleConfig> googleConfig, UserManager<Users> userManager,
                                 ITokenService tokenService)
        {
            _googleConfig = googleConfig.Value;
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<ApiResponse<LoginResponseDTO>> HandleGoogleLogin(GoogleLoginDTO model)
        {
            var payload = await this.VerifyGoogleToken(model.Token);
            if (payload == null)
            {
                var errors = new Dictionary<string, string> { { "Token", "Invalid google token." } };
                return ApiResponse<LoginResponseDTO>.Failed(errors, "Google Auth Failed.", HttpStatusCode.InternalServerError);
            }

            var user = await _userManager.FindByEmailAsync(payload.Email);
            if (user == null)
            {
                user = new Users
                {
                    UserName = payload.Email.Split('@').First().ToString(),
                    Email = payload.Email,
                    EmailConfirmed = true
                };
                await _userManager.CreateAsync(user);

                // Add user to the role
                var addToRole = await _userManager.AddToRoleAsync(user, UserRoles.User.ToString());
                if (!addToRole.Succeeded)
                {
                    var errors = new Dictionary<string, string> { { "User", "Failed to assign role to user" } };
                    return ApiResponse<LoginResponseDTO>.Failed(errors, "Register Failed.", HttpStatusCode.InternalServerError);
                }
            }

            #region generate and update jwt & refresh token with expiry in db
            var jwtToken = await _tokenService.GenerateJwtToken(user);
            var refreshToken = await _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.Now.AddDays(7);
            await _userManager.UpdateAsync(user);
            #endregion

            #region response map
            var response = new LoginResponseDTO
            {
                JwtToken = jwtToken,
                RefreshToken = refreshToken,
            };
            #endregion

            return ApiResponse<LoginResponseDTO>.Success(response, "User Validated");
        }

        private async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(string token)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new List<string> { _googleConfig.ClientId }
                };
                return await GoogleJsonWebSignature.ValidateAsync(token, settings);
            }
            catch { return null; }
        }
    }
}
