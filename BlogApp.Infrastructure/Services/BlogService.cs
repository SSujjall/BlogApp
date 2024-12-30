using Azure;
using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers;
using BlogApp.Application.Interface.IRepositories;
using BlogApp.Application.Interface.IServices;
using BlogApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Infrastructure.Services
{
    public class BlogService(IBlogRepository _blogRepository) : IBlogService
    {
        public async Task<ApiResponse<IEnumerable<BlogsDTO>>> GetAllBlogs()
        {
            var result = await _blogRepository.GetAll(null);
            if (result != null)
            {
                #region response model mapping
                var response = new List<BlogsDTO>();

                foreach (var item in result)
                {
                    var blog = new BlogsDTO
                    {
                        BlogId = item.BlogId,
                        UserId = item.UserId,
                        Title = item.Title,
                        Description = item.Description,
                        ImageUrl = item.ImageUrl,
                        UpVoteCount = item.UpVoteCount,
                        DownVoteCount = item.DownVoteCount,
                    };
                    response.Add(blog);
                }
                #endregion

                return ApiResponse<IEnumerable<BlogsDTO>>.Success(response, "All Blogs Listed");
            }
            return ApiResponse<IEnumerable<BlogsDTO>>.Failed(null, "Failed to load data");
        }

        public async Task<ApiResponse<BlogsDTO>> GetBlogById(int id)
        {
            var result = await _blogRepository.GetById(id);
            if (result != null)
            {
                #region response model mapping
                var response = new BlogsDTO
                {
                    BlogId = result.BlogId,
                    UserId = result.UserId,
                    Title = result.Title,
                    Description = result.Description,
                    ImageUrl = result.ImageUrl,
                    UpVoteCount = result.UpVoteCount,
                    DownVoteCount = result.DownVoteCount,
                };
                #endregion

                return ApiResponse<BlogsDTO>.Success(response, $"Blog id: {result.BlogId}");
            }

            return ApiResponse<BlogsDTO>.Failed(null, "Failed to load blog.");
        }

        public async Task<ApiResponse<BlogsDTO>> CreateBlog(string userId, CreateBlogDTO dto)
        {
            #region request model mapping
            var request = new Blogs
            {
                UserId = userId,
                Title = dto.Title,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
            };
            #endregion
            var result = await _blogRepository.Add(request);
            if (result != null)
            {
                #region response model mapping
                var response = new BlogsDTO
                {
                    BlogId = result.BlogId,
                    UserId = result.UserId,
                    Title = result.Title,
                    Description = result.Description,
                    ImageUrl = result.ImageUrl,
                    UpVoteCount = result.UpVoteCount,
                    DownVoteCount = result.DownVoteCount,
                };
                #endregion
                return ApiResponse<BlogsDTO>.Success(response, "");
            }
            return ApiResponse<BlogsDTO>.Failed(null, "Failed to create a new blog.");
        }
    }
}
