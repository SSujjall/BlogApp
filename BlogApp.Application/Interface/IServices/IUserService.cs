using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers;

namespace BlogApp.Application.Interface.IServices
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetAll();
        Task<string> DeleteUser(string id, List<string> errors);
        Task<string> UpdateUser(UpdateDTO updateUserDTO, List<string> errors);
        Task<ApiResponse<UserDTO>> GetUserById(string userId);
        Task<string> GetUserNameById(string userId);
    }
}
