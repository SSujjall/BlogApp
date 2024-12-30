using BlogApp.Domain.Entities;

namespace BlogApp.Application.Interface.IRepositories
{
    public interface IAuthRepository
    {
        public Task<Users> FindByUsername(string username);
        public Task<bool> CreateNewUser(Users user, string password);
        public Task<string> GetUserRole(Users user);
        Task<bool> UsernameExists(string username);
    }
}
