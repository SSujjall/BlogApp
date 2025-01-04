using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Interface.IServices
{
    public interface ICommentService
    {
        Task<ApiResponse<IEnumerable<CommentDTO>>> GetAllComment(int blogId);
        Task<ApiResponse<CommentDTO>> AddComment(AddCommentDTO dto);
    }
}
