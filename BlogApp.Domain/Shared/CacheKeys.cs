using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Domain.Shared
{
    public class CacheKeys
    {
        #region Blog Keys
        public const string GetAllBlogs = "GetAllBlogs";
        public const string GetAllBlogsPrefix = "GetAllBlogs*";
        public const string GetBlogById = "GetBlogById";
        #endregion
    }
}
