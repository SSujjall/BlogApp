using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers.HelperModels;

namespace BlogApp.Application.Interface.IServices
{
    public interface IUserService
    {
        Task<ApiResponse<IEnumerable<UserDTO>>> GetAllUsers();
        Task<ApiResponse<UserDTO>> GetUserById(string userId);
        Task<ApiResponse<string>> DeleteUser(string id);
        Task<ApiResponse<string>> ChangeUserPassword(UpdateUserPwDTO updateUserDTO);
        Task<ApiResponse<UserProfileDTO>> UpdateUserDetail(UserProfileReqDTO dto);
    }
}
