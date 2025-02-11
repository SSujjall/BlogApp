using System.Reflection.Metadata;
using BlogApp.Application.Interface.IRepositories;
using BlogApp.Domain.Entities;
using BlogApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using BlogApp.Application.Helpers.HelperModels;

namespace BlogApp.Infrastructure.Repositories
{
    public class BlogRepository : BaseRepository<Blogs>, IBlogRepository
    {
        private readonly AppDbContext _dbContext;
        public BlogRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(IEnumerable<Blogs>, int)> GetFilteredBlogs(GetRequest<Blogs> request)
        {
            var query = _dbContext.Blogs.AsQueryable().Where(x => x.IsDeleted == false);

            // Apply generic filters from GetRequest
            if (request.Filter != null)
            {
                query = query.Where(request.Filter);
            }
            // Calculate total count before pagination
            var totalCount = await query.CountAsync();

            // Apply blog-specific sorting logic
            if (!string.IsNullOrEmpty(request.SortBy))
            {
                switch (request.SortBy.ToLower())
                {
                    case "popularity":
                        query = query.OrderByDescending(b => b.UpVoteCount);
                        break;
                    case "recency":
                        query = query.OrderByDescending(b => b.CreatedAt);
                        break;
                    case "random":
                        query = query.OrderBy(_ => Guid.NewGuid());
                        break;
                    default:
                        query = query.OrderBy(b => b.BlogId); // Default sorting
                        break;
                }
            }
            // Apply pagination
            if (request.Skip.HasValue)
            {
                query = query.Skip(request.Skip.Value);
            }
            if (request.Take.HasValue)
            {
                query = query.Take(request.Take.Value);
            }
            var paginatedData = await query.ToListAsync();
            return (paginatedData, totalCount);
        }
    }
}
