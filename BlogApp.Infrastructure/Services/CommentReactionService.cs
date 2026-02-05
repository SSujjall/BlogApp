using AutoMapper;
using BlogApp.Application.DTOs;
using BlogApp.Application.Exceptions;
using BlogApp.Application.Helpers.HelperModels;
using BlogApp.Application.Interface.IRepositories;
using BlogApp.Application.Interface.IServices;
using BlogApp.Domain.Entities;
using BlogApp.Domain.Shared;
using System.Linq.Expressions;
using System.Net;

namespace BlogApp.Infrastructure.Services
{
    public class CommentReactionService(ICommentReactionRepository commentReactionRepo, IMapper mapper,
        ICommentService commentService, ITransactionService transactionService) : ICommentReactionService
    {
        protected readonly ICommentReactionRepository _commentReactionRepo = commentReactionRepo;
        private readonly IMapper _mapper = mapper;
        private readonly ICommentService _commentService = commentService;
        private readonly ITransactionService _tranService;

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
            return await _tranService.ExecuteInTransactionAsync(async () =>
            {
                Expression<Func<CommentReaction, bool>> filter = x => x.CommentId == model.CommentId && x.UserId == userId;
                var existingReaction = await _commentReactionRepo.FindSingleByConditionAsync(filter);
                if (existingReaction != null)
                {
                    var previousCommentVote = existingReaction.ReactionType;

                    if (previousCommentVote == model.ReactionType)
                    {
                        var errors = new Dictionary<string, string> { { "Same Vote", $"Cannot {previousCommentVote} again." } };
                        throw new ServiceException(errors, HttpStatusCode.BadRequest);
                    }

                    if (model.ReactionType == VoteType.None)
                    {
                        await _commentReactionRepo.Delete(existingReaction);
                    }
                    else
                    {
                        existingReaction.ReactionType = model.ReactionType;
                        await _commentReactionRepo.Update(existingReaction);
                    }

                    var success = await _commentService.UpdateCommentVoteCount(model, true, previousCommentVote); // Update vote count in main comment table
                    if (success == false)
                    {
                        var errors = new Dictionary<string, string> { { "ErrorCommentVote", $"Error occured when updating the blog's vote count" } };
                        throw new ServiceException(errors, HttpStatusCode.InternalServerError);
                    }

                    return ApiResponse<string>.Success(null, $"comment vote changed from {previousCommentVote.ToString()} to {model.ReactionType.ToString()}");
                }
                else
                {
                    // Cannot remove vote if it doesn't exist
                    if (model.ReactionType == VoteType.None)
                    {
                        var error = new Dictionary<string, string>
                        {
                            { "VoteNotFound", "Cannot remove a vote that doesn't exist." }
                        };
                        throw new ServiceException(error, HttpStatusCode.BadRequest);
                    }

                    var request = _mapper.Map<CommentReaction>(model);
                    request.UserId = userId; // set the userId to the one coming from parameter
                    var addCommentReactionRes = await _commentReactionRepo.AddAsync(request);
                    var updateCommentVoteRes = await _commentService.UpdateCommentVoteCount(model, false, null); // Update vote count in main comment table
                    if (addCommentReactionRes == null)
                    {
                        var error = new Dictionary<string, string>
                        {
                            { "ErrorAddingVote", "Error occured when adding comment vote" }
                        };
                        throw new ServiceException(error, HttpStatusCode.InternalServerError);
                    }
                    if (updateCommentVoteRes == false)
                    {
                        var error = new Dictionary<string, string>
                        {
                            { "ErrorUpdatingBlogVote", "Error occured when updating comment's vote" }
                        };
                        throw new ServiceException(error, HttpStatusCode.InternalServerError);
                    }
                    return ApiResponse<string>.Success(null, $"Comment voted as {request.ReactionType.ToString()}.");
                }
            });
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

        public async Task<ApiResponse<IEnumerable<GetAllUserCommentReactionDTO>>> GetAllUserCommentReactions(string userId)
        {
            Expression<Func<CommentReaction, bool>> filter = (x => x.UserId == userId);
            var commentReactions = await _commentReactionRepo.FindAllByConditionAsync(filter);
            if (commentReactions.Any())
            {
                var response = _mapper.Map<IEnumerable<GetAllUserCommentReactionDTO>>(commentReactions);
                return ApiResponse<IEnumerable<GetAllUserCommentReactionDTO>>.Success(response, "All User's Comment Votes Fetched Successfully.");
            }
            var errors = new Dictionary<string, string>
            {
                { "NotFound", "User has not voted any comments." }
            };
            return ApiResponse<IEnumerable<GetAllUserCommentReactionDTO>>.Failed(errors, "Vote Not Found.", System.Net.HttpStatusCode.NotFound);
        }
    }
}
