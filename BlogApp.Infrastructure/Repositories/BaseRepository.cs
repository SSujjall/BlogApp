using BlogApp.Application.Helpers.HelperModels;
using BlogApp.Application.Interface.IRepositories;
using BlogApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BlogApp.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly AppDbContext _dbContext;
        protected readonly DbSet<T> _dbSet; // for reaction repo

        public BaseRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        public async Task<T> Add(T entity)
        {
            var result = await _dbContext.Set<T>().AddAsync(entity);
            _dbContext.SaveChanges();
            return result.Entity;
        }

        public async Task Delete(T entity)
        {
            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<T> FindSingleByConditionAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(expression);
        }

        public async Task<IEnumerable<T>> GetAll(GetRequest<T>? request)
        {
            var query = _dbContext.Set<T>().AsQueryable();

            // Apply filter if provided
            if (request?.Filter != null)
            {
                query = query.Where(request.Filter);
            }
            // Apply ordering if provided
            if (request?.OrderBy != null)
            {
                query = request.OrderBy(query);
            }
            // Apply pagination if provided
            if (request?.Skip.HasValue == true)
            {
                query = query.Skip(request.Skip.Value);
            }
            if (request?.Take.HasValue == true)
            {
                query = query.Take(request.Take.Value);
            }

            // Execute the query and return the results
            var result = await query.ToListAsync();
            return result;
        }

        public async Task<T>? GetById(object entityId)
        {
            var result = await _dbContext.FindAsync<T>(entityId);
            return result;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<T> Update(T entity)
        {
            var result = _dbContext.Update(entity);
            await SaveChangesAsync();
            return result.Entity;
        }

        public async Task<IEnumerable<T>> FindAllByConditionAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbContext.Set<T>().Where(expression).ToListAsync();
        }
    }
}
