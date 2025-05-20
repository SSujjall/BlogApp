using System.Text.Json;
using System.Text.Json.Serialization;
using BlogApp.Infrastructure.Redis_Cache.Model;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace BlogApp.Infrastructure.Redis_Cache.Service
{
    /*
     * Redis is hosted in cloud.redis.io
     */
    public class RedisCache : IRedisCache, IDisposable
    {
        private readonly ISubscriber _subscriber;
        private readonly IDistributedCache _distributedCache;
        private readonly IConnectionMultiplexer _redisCache;
        //private readonly IDatabase _db;
        private readonly string _cacheInvalidationChannel = "cache-invalidation";

        public RedisCache(
            IDistributedCache distributedCache,
            IConnectionMultiplexer redisCache)
        {
            _distributedCache = distributedCache;
            _redisCache = redisCache;
            _subscriber = _redisCache.GetSubscriber();
            //_db = _redisCache.GetDatabase();

            /// Subscribe to cache invalidation messages
            _subscriber.Subscribe(_cacheInvalidationChannel, HandleCacheInvalidationMessage);
        }

        private void HandleCacheInvalidationMessage(RedisChannel channel, RedisValue msg)
        {
            try
            {
                var invalidationMsg = JsonSerializer.Deserialize<CacheInvalidationMessage>(msg);
                _distributedCache.Remove(invalidationMsg.Key);
            }
            catch (Exception ex)
            {
                // log here
            }
        }

        public async Task<T> GetOrCreateCache<T>(string key, Func<Task<T>> exec, TimeSpan expiry)
        {
            /// Json Settings for navigation property values
            var jsonOpts = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };

            //var x = await _db.StringGetAsync("testkey"); // Getting "string" type from redis

            //await _distributedCache.RemoveAsync(key);
            var cacheData = await _distributedCache.GetStringAsync(key); // gets "hash" type from redis
            if (!string.IsNullOrEmpty(cacheData))
                return JsonSerializer.Deserialize<T>(cacheData, jsonOpts)!;

            var data = await exec();
            if (data != null)
            {
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiry,
                };
                var datas = JsonSerializer.Serialize(data, jsonOpts);
                await _distributedCache.SetStringAsync(key, datas);
            }

            return data;
        }

        public async Task<T> UpdateDataAndInvalidateCache<T>(string key, Func<Task<T>> exec)
        {
            var updatedData = await exec();

            var invalidationMsg = new CacheInvalidationMessage
            {
                Key = key,
                Timestamp = DateTime.UtcNow
            };

            await _subscriber.PublishAsync(_cacheInvalidationChannel, JsonSerializer.Serialize(invalidationMsg));

            return updatedData;
        }

        public async Task RemoveKey(string key)
        {
            await _distributedCache.RemoveAsync(key);
        }

        public async Task DeleteKeysByPrefix(string prefix)
        {
            var server = _redisCache.GetServer(_redisCache.GetEndPoints().First());

            foreach (var key in server.Keys(pattern: $"{prefix}"))
            {
                await _distributedCache.RemoveAsync(key.ToString());
            }
        }

        public void Dispose()
        {
            _subscriber.Unsubscribe(_cacheInvalidationChannel);
        }
    }
}
