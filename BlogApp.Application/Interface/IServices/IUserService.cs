using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers.HelperModels;

namespace BlogApp.Application.Interface.IServices
{
    public interface IUserService
    {
        Task<ApiResponse<IEnumerable<UserDTO>>> GetAll();
        Task<ApiResponse<string>> DeleteUser(string id);
        Task<ApiResponse<UpdateUserDTO>> UpdateUser(UpdateUserDTO updateUserDTO, List<string> errors);
        Task<ApiResponse<UserDTO>> GetUserById(string userId);
    }
}
