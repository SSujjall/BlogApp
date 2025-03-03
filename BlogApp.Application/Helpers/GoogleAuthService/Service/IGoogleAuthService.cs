using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers.GoogleAuthService.Model;
using BlogApp.Application.Helpers.HelperModels;

namespace BlogApp.Application.Helpers.GoogleAuthService.Service
{
    public interface IGoogleAuthService
    {
        Task<ApiResponse<LoginResponseDTO>> HandleGoogleLogin(GoogleLoginDTO model);
    }
}
