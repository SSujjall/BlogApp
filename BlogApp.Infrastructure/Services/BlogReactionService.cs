using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
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
    public class BlogReactionService(IBlogReactionRepository blogReactionRepo, IMapper mapper) : IBlogReactionService
    {
        protected readonly IBlogReactionRepository _blogReactionRepo = blogReactionRepo;
        private readonly IMapper _mapper = mapper;

        public async Task<ApiResponse<IEnumerable<BlogReactionDTO>>> GetAllBlogVotes(int blogId)
        {
            var req = new GetRequest<BlogReaction> 
            { 
                Filter = r => r.BlogId == blogId 
            };
            var reactions = await _blogReactionRepo.GetAll(req);
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

        public async Task<ApiResponse<string>> UpvoteBlog(AddBlogReactionDTO model, string userId)
        {
            var req = _mapper.Map<BlogReaction>(model);

            Expression<Func<BlogReaction, bool>> filter = x => x.BlogId == model.BlogId && x.UserId == userId;
            var existingReaction = await _blogReactionRepo.FindSingleByConditionAsync(filter);
            if (existingReaction != null)
            {
                var previousBlogVote = existingReaction.ReactionType;
                existingReaction.ReactionType = model.ReactionType;
                await _blogReactionRepo.Update(existingReaction);
                return ApiResponse<string>.Success(null, $"blog vote changed from {previousBlogVote.ToString()} to {existingReaction.ReactionType.ToString()}");
            }
            else
            {
                return null;
            }
        }

        public Task<ApiResponse<string>> DownvoteBlog()
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<BlogReactionDTO>> GetVoteById()
        {
            throw new NotImplementedException();
        }
    }
}
