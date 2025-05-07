using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Infrastructure.Redis_Cache.Model
{
    public class CacheInvalidationMessage
    {
        public required string Key { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
