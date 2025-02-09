using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Interface.IRepositories
{
    public interface IReactionRepository<T> : IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetByUserIdAsync(string userId);
    }
}
