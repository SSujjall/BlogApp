using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers.HelperModels;
using Newtonsoft.Json.Linq;

namespace BlogApp.Application.Interface.IServices
{ 
    public interface IAuthService
    {
        Task<ApiResponse<RegisterResponseDTO>> RegisterUser(RegisterDTO registerDto);
        Task<ApiResponse<LoginResponseDTO>> LoginUser(LoginDTO loginDto);
        Task<ApiResponse<string>> ConfirmEmailVerification(string token, string email);
        Task<ApiResponse<ForgotPasswordResponseDTO>> GenerateForgotPasswordLink(string email);
        Task<ApiResponse<string>> ResetPasswordAsync(ResetPasswordDTO model);
        Task<ApiResponse<LoginResponseDTO>> RefreshToken(RefreshTokenRequestDTO model);
        Task<ApiResponse<string>> LogoutUser(string userId);
    }
}
