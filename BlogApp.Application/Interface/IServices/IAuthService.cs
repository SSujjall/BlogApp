using BlogApp.Application.DTOs;

namespace BlogApp.Application.Interface.IServices
{
    public interface IAuthService
    {
        Task<string> RegisterUser(RegisterDTO registerDto);
        Task<string> LoginUser(LoginDTO loginDto);
    }
}
