using BlogApp.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Interface.IRepositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task Delete(T entity);
        Task<IEnumerable<T>> GetAll(GetRequest<T>? request);
        Task<T>? GetById(object entityId);
        Task<T> FindByConditionAsync(Expression<Func<T, bool>> expression);
        Task SaveChangesAsync();
    }
}