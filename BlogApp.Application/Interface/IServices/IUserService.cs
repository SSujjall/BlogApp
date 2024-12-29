using BlogApp.Application.DTOs;

namespace BlogApp.Application.Interface.IServices
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetAll();
        Task<string> DeleteUser(string id, List<string> errors);
        Task<string> UpdateUser(UpdateDTO updateUserDTO, List<string> errors);
        Task<UserDTO> GetUserById(string userId);
        Task<string> GetUserNameById(string userId);
    }
}
