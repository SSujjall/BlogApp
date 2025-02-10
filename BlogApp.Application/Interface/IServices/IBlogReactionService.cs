using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers.HelperModels;
using BlogApp.Domain.Entities;

namespace BlogApp.Application.Interface.IServices
{
    public interface IBlogReactionService
    {
        Task<ApiResponse<IEnumerable<BlogReactionDTO>>> GetAllBlogVotes(int blogId);
        Task<ApiResponse<string>> UpvoteBlog(AddBlogReactionDTO model, string userId);
        Task<ApiResponse<string>> DownvoteBlog();
        Task<ApiResponse<BlogReactionDTO>> GetVoteById();
    }
}
