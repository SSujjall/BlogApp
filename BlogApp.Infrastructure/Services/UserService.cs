﻿using AutoMapper;
using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers.HelperModels;
using BlogApp.Application.Interface.IRepositories;
using BlogApp.Application.Interface.IServices;
using BlogApp.Domain.Entities;
using BlogApp.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<Users> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UserService(UserManager<Users> userManager, IUserRepository iuserRepository, AppDbContext context, IMapper mapper)
        {
            _userManager = userManager;
            _userRepository = iuserRepository;
            _context = context;
            _mapper = mapper;
        }

        public Task<ApiResponse<string>> DeleteUser(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<IEnumerable<UserDTO>>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<UserDTO>> GetUserById(string userId)
        {
            var result = await _userRepository.GetById(userId);
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

        public Task<string> UpdateUser(UpdateUserDTO updateUserDTO, List<string> errors)
        {
            throw new NotImplementedException();
        }

        Task<ApiResponse<UpdateUserDTO>> IUserService.UpdateUser(UpdateUserDTO updateUserDTO, List<string> errors)
        {
            throw new NotImplementedException();
        }
    }
}