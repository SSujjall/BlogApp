using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Domain.Shared
{
    public class CacheRequestItems
    {
        public string Id { get; set; } = string.Empty;
        public string Filter { get; set; } = string.Empty;
        public string Skip { get; set; } = "0";
        public string Take { get; set; } = "10";
        public string SortBy { get; set; } = string.Empty;
    }
}
