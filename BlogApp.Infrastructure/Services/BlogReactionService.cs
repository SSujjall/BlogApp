using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Core;
using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers.HelperModels;
using BlogApp.Application.Interface.IRepositories;
using BlogApp.Application.Interface.IServices;
using BlogApp.Domain.Entities;
using BlogApp.Domain.Shared;

namespace BlogApp.Infrastructure.Services
{
    public class BlogReactionService(IBlogReactionRepository blogReactionRepo, IMapper mapper, 
        IBlogService blogService) : IBlogReactionService
    {
        private readonly IBlogReactionRepository _blogReactionRepo = blogReactionRepo;
        private readonly IMapper _mapper = mapper;
        private readonly IBlogService _blogService = blogService;

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
            Expression<Func<BlogReaction, bool>> filter = (x => x.BlogId == model.BlogId && x.UserId == userId);
            var existingReaction = await _blogReactionRepo.FindSingleByConditionAsync(filter);
            if (existingReaction != null)
            {
                var previousVote = existingReaction.ReactionType;

                if (previousVote == model.ReactionType)
                {
                    var errors = new Dictionary<string, string> { { "Same Vote", $"Cannot {previousVote} again." } };
                    return ApiResponse<string>.Failed(errors, "Blog vote failed.");
                }

                if (model.ReactionType == VoteType.None)
                {
                    await _blogReactionRepo.Delete(existingReaction);
                }
                else
                {
                    existingReaction.ReactionType = model.ReactionType;
                    await _blogReactionRepo.Update(existingReaction);
                }

                await _blogService.UpdateBlogVoteCount(model, true, previousVote); // Update vote count in main blog table
                return ApiResponse<string>.Success(null, $"blog vote changed from {previousVote.ToString()} to {model.ReactionType.ToString()}");
            }
            else
            {
                if (model.ReactionType == VoteType.None)
                {
                    return ApiResponse<string>.Failed(null, "Cannot remove a vote that doesn't exist.");
                }

                try
                {
                    var request = _mapper.Map<BlogReaction>(model);
                    request.UserId = userId; // set the userId to the one coming from parameter
                    var response = await _blogReactionRepo.Add(request);
                    await _blogService.UpdateBlogVoteCount(model, false, null); // Update vote count in main blog table
                    if (response != null)
                    {
                        return ApiResponse<string>.Success(null, $"Blog voted as {request.ReactionType.ToString()}.");
                    }
                    return ApiResponse<string>.Failed(null, "Blog vote failed.");
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

            if (vote != null)
            {
                var response = new List<GetAllUserReactionDTO>();
                foreach (var item in vote)
                {
                    var reaction = _mapper.Map<GetAllUserReactionDTO>(item);
                    response.Add(reaction);
                }
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
