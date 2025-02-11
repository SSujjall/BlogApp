using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers.HelperModels;
using BlogApp.Domain.Entities;
using BlogApp.Domain.Shared;

namespace BlogApp.Application.Interface.IServices
{
    public interface IBlogService
    {
        Task<ApiResponse<IEnumerable<BlogsDTO>>> GetAllBlogs(GetRequest<Blogs> request);
        Task<ApiResponse<BlogsDTO>> GetBlogById(int id);
        Task<ApiResponse<BlogsDTO>> CreateBlog(string userId, CreateBlogDTO dto);
        Task<ApiResponse<string>> DeleteBlog(int id, string userId);
        Task<ApiResponse<BlogsDTO>> UpdateBlog(UpdateBlogDTO dto, string userId);
        Task UpdateBlogVoteCount(AddOrUpdateBlogReactionDTO model, bool reactionExists, VoteType? previousVote);
    }
}
