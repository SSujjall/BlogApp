using System.Linq.Expressions;
using System.Net;
using AutoMapper;
using BlogApp.Application.DTOs;
using BlogApp.Application.Exceptions;
using BlogApp.Application.Helpers.HelperModels;
using BlogApp.Application.Interface.IRepositories;
using BlogApp.Application.Interface.IServices;
using BlogApp.Domain.Entities;
using BlogApp.Domain.Shared;

namespace BlogApp.Infrastructure.Services
{
    public class BlogReactionService(IBlogReactionRepository blogReactionRepo, IMapper mapper, 
        IBlogService blogService, ITransactionService tranService) : IBlogReactionService
    {
        private readonly IBlogReactionRepository _blogReactionRepo = blogReactionRepo;
        private readonly IMapper _mapper = mapper;
        private readonly IBlogService _blogService = blogService;
        private readonly ITransactionService _tranService = tranService;

        public async Task<ApiResponse<IEnumerable<BlogReactionDTO>>> GetAllBlogVotes(int blogId)
        {
            Expression<Func<BlogReaction, bool>> req = x => x.BlogId == blogId;
            var reactions = await _blogReactionRepo.FindAllByConditionAsync(req);
            if (reactions.Any())
            {
                var response = _mapper.Map<IEnumerable<BlogReactionDTO>>(reactions);
                return ApiResponse<IEnumerable<BlogReactionDTO>>.Success(response, "Reactions for blog retrieved successfully.");
            }
            var errors = new Dictionary<string, string>
            {
                { "No Data", "No Reactions found for blog." }
            };
            return ApiResponse<IEnumerable<BlogReactionDTO>>.Failed(errors, "No reactions found.", System.Net.HttpStatusCode.NotFound);
        }

        public async Task<ApiResponse<string>> VoteBlog(AddOrUpdateBlogReactionDTO model, string userId)
        {
            return await _tranService.ExecuteInTransactionAsync(async () =>
            {
                Expression<Func<BlogReaction, bool>> filter = (x => x.BlogId == model.BlogId && x.UserId == userId);
                var existingReaction = await _blogReactionRepo.FindSingleByConditionAsync(filter);
                if (existingReaction != null)
                {
                    var previousVote = existingReaction.ReactionType;

                    // Prevent voting the same reaction twice
                    if (previousVote == model.ReactionType)
                    {
                        var errors = new Dictionary<string, string> { { "SameVote", $"Cannot {previousVote} again." } };
                        throw new ServiceException(errors, HttpStatusCode.BadRequest);
                    }

                    // Remove vote if user sets it to None
                    if (model.ReactionType == VoteType.None)
                    {
                        await _blogReactionRepo.Delete(existingReaction);
                    }
                    else
                    {
                        // Update existing reaction
                        existingReaction.ReactionType = model.ReactionType;
                        await _blogReactionRepo.Update(existingReaction);
                    }

                    // Update vote counts in main blog table
                    var success = await _blogService.UpdateBlogVoteCount(model, true, previousVote);
                    if (success == false)
                    {
                        var errors = new Dictionary<string, string> { { "ErrorBlogVote", $"Error occured when updating the blog's vote count" } };
                        throw new ServiceException(errors, HttpStatusCode.InternalServerError);
                    }

                    return ApiResponse<string>.Success(null, $"blog vote changed from {previousVote.ToString()} to {model.ReactionType.ToString()}");
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

                    // Add new reaction
                    var request = _mapper.Map<BlogReaction>(model);
                    request.UserId = userId; // set the userId to the one coming from parameter
                    var addBlogReactonRes = await _blogReactionRepo.AddAsync(request);
                    var updateBlogVoteRes = await _blogService.UpdateBlogVoteCount(model, false, null); // Update vote count in main blog table
                    if (addBlogReactonRes == null)
                    {
                        var error = new Dictionary<string, string>
                        {
                            { "ErrorAddingVote", "Error occured when adding vote" }
                        };
                        throw new ServiceException(error, HttpStatusCode.InternalServerError);
                    }
                    if (updateBlogVoteRes == false)
                    {
                        var error = new Dictionary<string, string>
                        {
                            { "ErrorUpdatingBlogVote", "Error occured when updating blog's vote" }
                        };
                        throw new ServiceException(error, HttpStatusCode.InternalServerError);
                    }
                    return ApiResponse<string>.Success(null, $"Blog voted as {request.ReactionType.ToString()}.");
                    
                }
            });
        }

        public async Task<ApiResponse<BlogReactionDTO>> GetBlogVoteById(int id)
        {
            Expression<Func<BlogReaction, bool>> filter = x => x.Id == id;
            var vote = await _blogReactionRepo.FindSingleByConditionAsync(filter);

            if (vote != null)
            {
                var response = _mapper.Map<BlogReactionDTO>(vote);
                return ApiResponse<BlogReactionDTO>.Success(response, "Vote Fetched Successfully.");
            }

            var errors = new Dictionary<string, string>
            {
                { "NotFound", "Vote not found." }
            };
            return ApiResponse<BlogReactionDTO>.Failed(errors, "Vote Not Found.", System.Net.HttpStatusCode.NotFound);
        }

        public async Task<ApiResponse<IEnumerable<GetAllUserReactionDTO>>> GetAllUserReactionsByUserId(string userId)
        {
            Expression<Func<BlogReaction, bool>> filter = x => x.UserId == userId;
            var vote = await _blogReactionRepo.FindAllByConditionAsync(filter);

            if (vote.Any())
            {
                var response = _mapper.Map<IEnumerable<GetAllUserReactionDTO>>(vote);
                return ApiResponse<IEnumerable<GetAllUserReactionDTO>>.Success(response, "All User's Blog Votes Fetched Successfully.");
            }

            var errors = new Dictionary<string, string>
            {
                { "NotFound", "User has not voted anything." }
            };
            return ApiResponse<IEnumerable<GetAllUserReactionDTO>>.Failed(errors, "Vote Not Found.", System.Net.HttpStatusCode.NotFound);
        }
    }
}
