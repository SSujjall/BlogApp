using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers;
using BlogApp.Application.Interface.IRepositories;
using BlogApp.Application.Interface.IServices;
using BlogApp.Domain.Entities;
using BlogApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<Users> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly AppDbContext _context;

        public UserService(UserManager<Users> userManager, IUserRepository iuserRepository, AppDbContext context)
        {
            _userManager = userManager;
            _userRepository = iuserRepository;
            _context = context;

        }

        public Task<string> DeleteUser(string id, List<string> errors)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<UserDTO>> GetUserById(string userId)
        {
            var result = await _userRepository.GetById(userId);
            if (result != null)
            {
                #region response model mapping
                var response = new UserDTO
                {
                    Username = result.UserName,
                    Email = result.Email,
                };
                #endregion
                return ApiResponse<UserDTO>.Success(response, "User Data Loaded.");
            }
            var errors = new Dictionary<string, string>() { { "User", "User Not Found." } };
            return ApiResponse<UserDTO>.Failed(errors, "User Fetch Failed.");
        }

        public Task<string> GetUserNameById(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateUser(UpdateDTO updateUserDTO, List<string> errors)
        {
            throw new NotImplementedException();
        }
    }
}