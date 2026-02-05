using BlogApp.Application.Helpers.HelperModels;
using System.Linq.Expressions;

namespace BlogApp.Application.Interface.IRepositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);
        Task<T> Update(T entity);
        Task Delete(T entity);
        Task<IEnumerable<T>> GetAllAsync(GetRequest<T>? request);
        Task<T?> GetByIdAsync(object entityId);
        Task<T> FindSingleByConditionAsync(Expression<Func<T, bool>> expression);
        public Task<IEnumerable<T>> FindAllByConditionAsync(Expression<Func<T, bool>> expression);
        Task SaveChangesAsync();
    }
}