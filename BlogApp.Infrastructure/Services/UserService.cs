using AutoMapper;
using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers.HelperModels;
using BlogApp.Application.Interface.IRepositories;
using BlogApp.Application.Interface.IServices;
using BlogApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<Users> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(UserManager<Users> userManager, IUserRepository iuserRepository, IMapper mapper)
        {
            _userManager = userManager;
            _userRepository = iuserRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<UserDTO>>> GetAllUsers()
        {
            var result = await _userRepository.GetAllAsync(null);
            if (result.Any())
            {
                var response = _mapper.Map<IEnumerable<UserDTO>>(result);
                return ApiResponse<IEnumerable<UserDTO>>.Success(response, "Users fetched");
            }
            var errors = new Dictionary<string, string>() { { "User", "User Not Found." } };
            return ApiResponse<IEnumerable<UserDTO>>.Failed(errors, "User Fetch Failed.");
        }

        public async Task<ApiResponse<UserDTO>> GetUserById(string userId)
        {
            var result = await _userRepository.GetByIdAsync(userId);
            if (result != null)
            {
                #region response model mapping
                var responseModel = _mapper.Map<UserDTO>(result);
                #endregion
                return ApiResponse<UserDTO>.Success(responseModel, "User Data Loaded.");
            }
            var errors = new Dictionary<string, string>() { { "User", "User Not Found." } };
            return ApiResponse<UserDTO>.Failed(errors, "User Fetch Failed.");
        }

        public Task<ApiResponse<string>> DeleteUser(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<string>> ChangeUserPassword(UpdateUserPwDTO updatePwDto)
        {
            Dictionary<string, string> errors;

            var user = await _userRepository.GetByIdAsync(updatePwDto.UserId);
            if (user == null)
            {
                errors = new Dictionary<string, string>() { { "User", "User Not Found" } };
                return ApiResponse<string>.Failed(errors, "User Fetch Failed.");
            }

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, updatePwDto.CurrentPassword);
            if (!isPasswordCorrect)
            {
                errors = new Dictionary<string, string>() { { "Password", "Incorrect current password" } };
                return ApiResponse<string>.Failed(errors, "Password change failed.");
            }

            if (updatePwDto.CurrentPassword == updatePwDto.NewPassword)
            {
                errors = new Dictionary<string, string>() { { "Password", "Old and new password cannot be same." } };
                return ApiResponse<string>.Failed(errors, "Password change failed.");
            }

            var response = await _userManager.ChangePasswordAsync(user, updatePwDto.CurrentPassword, updatePwDto.NewPassword);
            if (response.Succeeded)
            {
                return ApiResponse<string>.Success(null, "Password changed successfully.");
            }
            errors = new Dictionary<string, string>() { { "Password", "Internal error occured" } };
            return ApiResponse<string>.Failed(errors, "Password change failed.");
        }

        public async Task<ApiResponse<UserProfileDTO>> UpdateUserDetail(UserProfileReqDTO dto)
        {
            Dictionary<string, string> errors;

            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null)
            {
                errors = new Dictionary<string, string>() { { "User", "User Not Found" } };
                return ApiResponse<UserProfileDTO>.Failed(errors, "User Fetch Failed.");
            }

            if (dto.Username != null) user.UserName = dto.Username;
            if (dto.Email != null) user.Email = dto.Email;
            if (dto.PhoneNumber != null) user.PhoneNumber = dto.PhoneNumber;

            var response = await _userManager.UpdateAsync(user);
            if (response.Succeeded)
            {
                var responseModel = _mapper.Map<UserProfileDTO>(user);
                return ApiResponse<UserProfileDTO>.Success(responseModel, "User data updated successfully.");
            }

            errors = new Dictionary<string, string>() { { "User", "Internal error occured when updating user" } };
            return ApiResponse<UserProfileDTO>.Failed(errors, "User Update Failed.");
        }
    }
}