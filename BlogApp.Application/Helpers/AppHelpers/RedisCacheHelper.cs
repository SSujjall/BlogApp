using System.Text.Json;
using BlogApp.Domain.Shared;

namespace BlogApp.Application.Helpers.AppHelpers
{
    public static class RedisCacheHelper
    {
        public static string GenerateCacheKey(string baseKey, CacheRequestItems filters)
        {
            var exFil = $"{baseKey}";
            exFil += "-ID:" + filters.Id;
            exFil += "-Filter:" + filters.Filter;
            exFil += "-Skip:" + filters.Skip;
            exFil += "-Take:" + filters.Take;
            exFil += "-SortBy:" + filters.SortBy;

            return exFil;

            //var serializedFields = JsonSerializer.Serialize(filters);
            //return $"{baseKey}:{serializedFields}";
        }
    }
}
