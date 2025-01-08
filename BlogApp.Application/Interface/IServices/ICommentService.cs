using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Interface.IServices
{
    public interface ICommentService
    {
        Task<ApiResponse<IEnumerable<CommentDTO>>> GetAllCommentByBlogId(int blogId);
        Task<ApiResponse<CommentDTO>> AddComment(AddCommentDTO dto, string userId);
        Task<ApiResponse<CommentDTO>> UpdateComment(UpdateCommentDTO dto, string userId);
        Task<ApiResponse<string>> DeleteComment(int commentId, string userId);
    }
}
