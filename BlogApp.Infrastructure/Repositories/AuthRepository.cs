using BlogApp.Application.Interface.IRepositories;
using BlogApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<Users> _userManager;

        public AuthRepository(UserManager<Users> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> CreateNewUser(Users user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return result.Succeeded ? true : false;
        }

        public async Task<Users> FindByUsername(string username)
        {
            var userExists = await _userManager.FindByNameAsync(username);
            return userExists;
        }

        public async Task<string> GetUserRole(Users user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            return userRoles.FirstOrDefault();
        }

        public async Task<bool> UsernameExists(string username)
        {
            var userExists = await _userManager.FindByNameAsync(username);
            return userExists != null ? true : false;
        }

        public async Task<bool> EmailExists(string email)
        {
            var emailExists = await _userManager.FindByEmailAsync(email);
            return emailExists != null ? true : false;
        }
    }
}
