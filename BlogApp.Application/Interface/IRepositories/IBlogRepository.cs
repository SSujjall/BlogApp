using System.Reflection.Metadata;
using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers.HelperModels;
using BlogApp.Domain.Entities;
using BlogApp.Domain.Shared;

namespace BlogApp.Application.Interface.IRepositories
{
    public interface IBlogRepository : IBaseRepository<Blogs>
    {
        public Task<GetFilteredBlogsDTO> GetFilteredBlogs(GetRequest<Blogs> request);
    }
}
