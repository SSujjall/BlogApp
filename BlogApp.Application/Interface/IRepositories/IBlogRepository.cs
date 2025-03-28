﻿using System.Reflection.Metadata;
using BlogApp.Application.Helpers.HelperModels;
using BlogApp.Domain.Entities;
using BlogApp.Domain.Shared;

namespace BlogApp.Application.Interface.IRepositories
{
    public interface IBlogRepository : IBaseRepository<Blogs>
    {
        public Task<(IEnumerable<Blogs>, int)> GetFilteredBlogs(GetRequest<Blogs> request);
    }
}
