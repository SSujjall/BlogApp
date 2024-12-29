using BlogApp.Application.Interface.IRepositories;
using BlogApp.Domain.Entities;
using BlogApp.Infrastructure.Persistence;

namespace BlogApp.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _dbContext = appDbContext;
        }
    }
}