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
        public const string GetAllBlogsNameForHelper = "GetAllBlogs";
        public const string GetBlogById = "GetBlogById";

        // when using DeleteKeysByPrefix of redisCache service, you need to have
        // a '*' wildcard if you are deleting many keys according to pattern
        public const string GetAllBlogsPrefix = "GetAllBlogs*"; 
        #endregion
    }
}
