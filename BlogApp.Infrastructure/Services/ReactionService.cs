using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers.HelperModels;
using BlogApp.Application.Interface.IRepositories;
using BlogApp.Application.Interface.IServices;
using BlogApp.Domain.Entities;
using BlogApp.Domain.Entities.Abstracts;

namespace BlogApp.Infrastructure.Services
{
    public class ReactionService<T> : IReactionService<T> where T : Reaction
    {
        private readonly IReactionRepository<T> _reactionRepository;

        public ReactionService(IReactionRepository<T> reactionRepository)
        {
            _reactionRepository = reactionRepository;
        }

        public async Task<ApiResponse<IEnumerable<ReactionDTO>>> GetAllReactions()
        {
            var reactions = await _reactionRepository.GetAll(null);

            if (reactions != null)
            {
                #region Response Map
                var response = reactions.Select(r => new ReactionDTO
                {
                    EntityName = r is BlogReaction ? "BlogReaction" : "CommentReaction",
                    EntityId = r is BlogReaction ? ((BlogReaction)(object)r).BlogId : ((CommentReaction)(object)r).CommentId, // Choose the correct ID based on type
                    UserId = r.UserId,
                    ReactionType = r.ReactionType,
                    CreatedAt = r.CreatedAt
                });
                #endregion

                return ApiResponse<IEnumerable<ReactionDTO>>.Success(response, "Reactions retrieved successfully.");
            }
            return ApiResponse<IEnumerable<ReactionDTO>>.Failed(
                new Dictionary<string, string> { { "NoData", "No reactions found." } },
                "No reactions found.",
                HttpStatusCode.NotFound
            );
        }

        public async Task<ApiResponse<ReactionDTO>> GetReactionById(int id)
        {
            var reaction = await _reactionRepository.GetById(id);

            if (reaction != null)
            {
                #region Response Map
                var response = new ReactionDTO
                {
                    EntityId = reaction is BlogReaction ? ((BlogReaction)(object)reaction).BlogId : ((CommentReaction)(object)reaction).CommentId,
                    UserId = reaction.UserId,
                    ReactionType = reaction.ReactionType,
                    CreatedAt = reaction.CreatedAt
                };
                #endregion

                return ApiResponse<ReactionDTO>.Success(response, "Reaction retrieved successfully.");
            }

            return ApiResponse<ReactionDTO>.Failed(
                new Dictionary<string, string> { { "NotFound", "Reaction not found." } },
                "Reaction not found.",
                HttpStatusCode.NotFound
            );
        }

        public async Task<ApiResponse<ReactionDTO>> AddReaction(T reaction)
        {
            try
            {
                await _reactionRepository.Add(reaction);

                #region Response Map
                var response = new ReactionDTO
                {
                    EntityId = reaction is BlogReaction ? ((BlogReaction)(object)reaction).BlogId : ((CommentReaction)(object)reaction).CommentId,
                    UserId = reaction.UserId,
                    ReactionType = reaction.ReactionType,
                    CreatedAt = reaction.CreatedAt
                };
                #endregion

                return ApiResponse<ReactionDTO>.Success(response, "Reaction added successfully.", HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return ApiResponse<ReactionDTO>.Failed(
                    new Dictionary<string, string> { { "Error", ex.Message } },
                    "Failed to add reaction.",
                    HttpStatusCode.InternalServerError
                );
            }
        }

        public async Task<ApiResponse<string>> RemoveReaction(int id, string userId)
        {
            var reaction = await _reactionRepository.GetById(id);
            if (reaction != null)
            {
                if (reaction.UserId != userId)
                {
                    var authError = new Dictionary<string, string>() { { "Authorization", "You are not authorized to delete this reaction." } };
                    return ApiResponse<string>.Failed(authError, "Unauthorized reaction deletion attempt.");
                }
                try
                {
                    await _reactionRepository.Delete(reaction);
                    return ApiResponse<string>.Success(null, "Reaction removed.");
                }
                catch (Exception ex)
                {
                    var exceptionErrors = new Dictionary<string, string>() { { "Exception", ex.Message } };
                    return ApiResponse<string>.Failed(exceptionErrors, "Reaction Deletion Failed Due to an Error");
                }
            }

            return ApiResponse<string>.Failed(
                new Dictionary<string, string> { { "NotFound", "Reaction not found." } },
                "Reaction not found.",
                HttpStatusCode.NotFound
            );
        }
    }
}
