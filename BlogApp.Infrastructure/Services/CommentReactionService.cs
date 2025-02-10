using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers.HelperModels;
using BlogApp.Application.Interface.IRepositories;
using BlogApp.Application.Interface.IServices;
using BlogApp.Domain.Entities;

namespace BlogApp.Infrastructure.Services
{
    public class CommentReactionService(ICommentReactionRepository commentReactionRepo, IMapper mapper) : ICommentReactionService
    {
        protected readonly ICommentReactionRepository _commentReactionRepo = commentReactionRepo;
        private readonly IMapper _mapper = mapper;

        public async Task<ApiResponse<IEnumerable<CommentReactionDTO>>> GetAllCommentVotes(int commentId)
        {
            Expression<Func<CommentReaction, bool>> req = x => x.CommentId == commentId;
            var reactions = await _commentReactionRepo.FindAllByConditionAsync(req);
            if (reactions.Any())
            {
                var response = _mapper.Map<IEnumerable<CommentReactionDTO>>(reactions);
                return ApiResponse<IEnumerable<CommentReactionDTO>>.Success(response, "Reactions for comment retrieved successfully.");
            }
            var errors = new Dictionary<string, string>
            {
                { "No Data", "No Reactions found for comment." }
            };
            return ApiResponse<IEnumerable<CommentReactionDTO>>.Failed(errors, "No reactions found.", System.Net.HttpStatusCode.NotFound);
        }

        public async Task<ApiResponse<string>> VoteComment(AddOrUpdateCommentReactionDTO model, string userId)
        {
            Expression<Func<CommentReaction, bool>> filter = x => x.CommentId == model.CommentId && x.UserId == userId;
            var existingReaction = await _commentReactionRepo.FindSingleByConditionAsync(filter);
            if (existingReaction != null)
            {
                var previousCommentVote = existingReaction.ReactionType;
                existingReaction.ReactionType = model.ReactionType;
                await _commentReactionRepo.Update(existingReaction);
                return ApiResponse<string>.Success(null, $"comment vote changed from {previousCommentVote.ToString()} to {existingReaction.ReactionType.ToString()}");
            }
            else
            {
                try
                {
                    var request = _mapper.Map<CommentReaction>(model);
                    request.UserId = userId; // set the userId to the one coming from parameter
                    var response = await _commentReactionRepo.Add(request);
                    if (response != null)
                    {
                        return ApiResponse<string>.Success(null, $"Comment voted as {request.ReactionType.ToString()}.");
                    }
                    return ApiResponse<string>.Failed(null, "Comment vote failed.");
                }
                catch (Exception e)
                {
                    var errors = new Dictionary<string, string>
                    {
                        { "Exception", $"{e.Message}" }
                    };
                    return ApiResponse<string>.Failed(errors, "Exception Occured.");
                }
            }
        }

        public async Task<ApiResponse<CommentReactionDTO>> GetCommentVoteById(int id)
        {
            Expression<Func<CommentReaction, bool>> filter = x => x.Id == id;
            var vote = await _commentReactionRepo.FindSingleByConditionAsync(filter);

            if (vote != null)
            {
                var response = _mapper.Map<CommentReactionDTO>(vote);
                return ApiResponse<CommentReactionDTO>.Success(response, "Vote Fetched Successfully.");
            }

            var errors = new Dictionary<string, string>
            {
                { "NotFound", "Vote not found." }
            };
            return ApiResponse<CommentReactionDTO>.Failed(errors, "Vote Not Found.", System.Net.HttpStatusCode.NotFound);
        }
    }
}
