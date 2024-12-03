using BlogApp.Application.Interface.IRepositories;
using BlogApp.Domain.Entities;
using BlogApp.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Infrastructure.Repositories
{
    public class BlogRepository : BaseRepository<Blogs>, IBlogRepository
    {
        private readonly AppDbContext _appDbContext;
        public BlogRepository(AppDbContext dbContext) : base(dbContext)
        {
            _appDbContext = dbContext;
        }
    }
}
