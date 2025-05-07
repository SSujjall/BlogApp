using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlogApp.Application.Helpers.AppHelpers
{
    public static class RedisCacheHelper
    {
        public static string GenerateCacheKey(string baseKey, object filters)
        {
            var serializedFields = JsonSerializer.Serialize(filters);
            return $"{baseKey}:{serializedFields}";
        }
    }
}
