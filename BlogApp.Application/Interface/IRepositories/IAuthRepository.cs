using BlogApp.Domain.Entities;

namespace BlogApp.Application.Interface.IRepositories
{
    public interface IAuthRepository
    {
        public Task<User> FindByUsername(string username);
        public Task<bool> CreateNewUser(User user, string password);
        public Task<string> GetUserRole(User user);
        Task<bool> UsernameExists(string username);
    }
}
