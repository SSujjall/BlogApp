using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogApp.Application.Interface.IRepositories;
using BlogApp.Domain.Entities.Abstracts;
using BlogApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Infrastructure.Repositories
{
    public class ReactionRepository<T> : BaseRepository<T>, IReactionRepository<T> where T : Reaction
    {
        private readonly AppDbContext _dbContext;
        public ReactionRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<T>> GetByUserIdAsync(string userId)
        {
            return await _dbSet.Where(r => r.UserId == userId).ToListAsync();
        }
    }
}
