using BlogApp.Application.Helpers.HelperModels;
using System.Linq.Expressions;

namespace BlogApp.Application.Interface.IRepositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task Delete(T entity);
        Task<IEnumerable<T>> GetAll(GetRequest<T>? request);
        Task<T>? GetById(object entityId);
        Task<T> FindSingleByConditionAsync(Expression<Func<T, bool>> expression);
        public Task<IEnumerable<T>> FindAllByConditionAsync(Expression<Func<T, bool>> expression);
        Task SaveChangesAsync();
    }
}