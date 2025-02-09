using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Interface.IServices
{
    public interface IReactionService<T> where T : class
    {
        Task<IEnumerable<T>> GetAllReactionsAsync();
        Task<T?> GetReactionByIdAsync(int id);
        Task AddReactionAsync(T reaction);
        Task RemoveReactionAsync(int id);
    }
}
