using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers.HelperModels;

namespace BlogApp.Application.Interface.IServices
{
    public interface ICommentReactionService
    {
        Task<ApiResponse<IEnumerable<CommentReactionDTO>>> GetAllCommentVotes(int commentId);
        Task<ApiResponse<string>> VoteComment(AddOrUpdateCommentReactionDTO model, string userId);
        Task<ApiResponse<CommentReactionDTO>> GetCommentVoteById(int id);
    }
}
