using BlogApp.Application.Interface.IRepositories;
using BlogApp.Domain.Entities;
using BlogApp.Infrastructure.Persistence;

namespace BlogApp.Infrastructure.Repositories
{
    public class BlogRepository : BaseRepository<Blogs>, IBlogRepository
    {
        private readonly AppDbContext _dbContext;
        public BlogRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
