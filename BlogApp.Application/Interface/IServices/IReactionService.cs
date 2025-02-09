using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers.HelperModels;

namespace BlogApp.Application.Interface.IServices
{
    public interface IReactionService<T> where T : class
    {
        Task<ApiResponse<IEnumerable<ReactionDTO>>> GetAllReactions();
        Task<ApiResponse<ReactionDTO>> GetReactionById(int id);
        Task<ApiResponse<ReactionDTO>> AddReaction(T reaction);
        Task<ApiResponse<string>> RemoveReaction(int id, string userId);
    }
}
