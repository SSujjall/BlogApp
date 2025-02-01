using System.Linq.Expressions;
using System.Security.Permissions;

namespace BlogApp.Application.Helpers.HelperModels
{
    public class GetRequest<T> where T : class
    {
        public Expression<Func<T, bool>> Filter { get; set; }
        public Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public string? SortBy { get; set; }
    }
}
