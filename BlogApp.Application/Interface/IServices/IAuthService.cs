using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers.HelperModels;

namespace BlogApp.Application.Interface.IServices
{
    public interface IAuthService
    {
        Task<ApiResponse<string>> RegisterUser(RegisterDTO registerDto);
        Task<ApiResponse<object>> LoginUser(LoginDTO loginDto);
    }
}
