using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogApp.Application.Interface.IRepositories;
using BlogApp.Domain.Entities;
using BlogApp.Infrastructure.Persistence;

namespace BlogApp.Infrastructure.Repositories
{
    public class BlogReactionRepository : BaseRepository<BlogReaction>, IBlogReactionRepository
    {
        public BlogReactionRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
