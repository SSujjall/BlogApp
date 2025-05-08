using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Infrastructure.Redis_Cache.Service
{
    public interface IRedisCache
    {
        Task<T> GetOrCreateCache<T>(string key, Func<Task<T>> exec, TimeSpan expiry);
        Task<T> UpdateDataAndInvalidateCache<T>(string key, Func<Task<T>> exec);
        Task RemoveKey(string key);
        Task DeleteKeysByPrefix(string prefix);
    }
}
