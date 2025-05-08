using System.Text.Json;
using BlogApp.Domain.Shared;

namespace BlogApp.Application.Helpers.AppHelpers
{
    public static class RedisCacheHelper
    {
        public static string GenerateCacheKey(string baseKey, CacheRequestItems filters)
        {
            //var exFil = $"{baseKey}";
            //if (!string.IsNullOrEmpty(filters.Id))
            //    exFil += ":" + filters.Id;
            //if (!string.IsNullOrEmpty(filters.Filter))
            //    exFil += ":" + filters.Filter;
            //if (!string.IsNullOrEmpty(filters.Skip))
            //    exFil += ":" + filters.Skip;
            //if (!string.IsNullOrEmpty(filters.Take))
            //    exFil += ":" + filters.Take;
            //if (!string.IsNullOrEmpty(filters.SortBy))
            //    exFil += ":" + filters.SortBy;

            var serializedFields = JsonSerializer.Serialize(filters);
            return $"{baseKey}:{serializedFields}";
        }
    }
}
