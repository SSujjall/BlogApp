using AutoMapper;
using Azure;
using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers;
using BlogApp.Application.Helpers.HelperModels;
using BlogApp.Application.Interface.IRepositories;
using BlogApp.Application.Interface.IServices;
using BlogApp.Domain.Entities;
using BlogApp.Domain.Shared;
using BlogApp.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Infrastructure.Services
{
    public class CommentService(ICommentRepository _commentRepository, IBaseRepository<CommentHistory> _commentHistoryRepo, 
        IMapper _mapper, IUserRepository _userRepository, IBlogService _blogService) : ICommentService
    {
        public async Task<ApiResponse<IEnumerable<CommentDTO>>> GetAllCommentByBlogId(int blogId)
        {
            var requestFilter = new GetRequest<Comments>
            {
                Filter = (x => x.BlogId == blogId && x.IsDeleted == false)
            };

            var result = await _commentRepository.GetAll(requestFilter);
            if (result != null)
            {
                #region response model mapping (using LINQ instead of normal foreach loop)
                var response = result.Select(comment => new CommentDTO
                {
                    CommentId = comment.CommentId,
                    BlogId = comment.BlogId,
                    User = new CommentUser
                    {
                        UserId = comment.UserId,
                        Name = comment.User.UserName ?? "",
                    },
                    CommentDescription = comment.CommentDescription,
                    CreatedAt = comment.CreatedAt,
                    UpdatedAt = comment.UpdatedAt
                });
                #endregion
                return ApiResponse<IEnumerable<CommentDTO>>.Success(response, "All Comments of the blog fetched.");
            }
            return ApiResponse<IEnumerable<CommentDTO>>.Failed(null, "No comments to load.");
        }

        public async Task<ApiResponse<CommentDTO>> AddComment(AddCommentDTO dto, string userId)
        {
            #region request model mapping
            var request = new Comments
            {
                BlogId = dto.BlogId,
                UserId = userId,
                CommentDescription = dto.CommentDescription,
            };
            #endregion

            var result = await _commentRepository.Add(request);
            if (result != null)
            {
                await _blogService.UpdateBlogCommentCount(dto.BlogId, true); // Increase comment count
                #region response model mapping
                var userDetail = await _userRepository.GetById(result.UserId); // create a separate method in repo for just getting username and userId using result.UserId instead of fetching everything.
                var response = new CommentDTO
                {
                    CommentId = result.CommentId,
                    BlogId = result.BlogId,
                    User = new CommentUser
                    {
                        UserId = result.UserId,
                        Name = userDetail.UserName
                    },
                    CommentDescription = result.CommentDescription,
                    CreatedAt = result.CreatedAt,
                    UpdatedAt = result.UpdatedAt
                };
                #endregion
                return ApiResponse<CommentDTO>.Success(response, "Comment Added Successfully");
            }
            return ApiResponse<CommentDTO>.Failed(null, "Failed to add comment");
        }

        public async Task<ApiResponse<CommentDTO>> UpdateComment(UpdateCommentDTO dto, string userId)
        {
            var existingComment = await _commentRepository.GetById(dto.CommentId);
            if (existingComment != null)
            {
                if (existingComment.UserId != userId)
                {
                    var authError = new Dictionary<string, string>() { { "Authorization", "You are not authorized to update this comment." } };
                    return ApiResponse<CommentDTO>.Failed(authError, "Unauthorized blog update attempt.");
                }

                #region Add to Comment History
                var historyReq = _mapper.Map<CommentHistory>(existingComment);
                historyReq.UpdatedAt = DateTime.Now;
                var historyRes = await _commentHistoryRepo.Add(historyReq);
                if (historyRes == null)
                {
                    var error = new Dictionary<string, string>() { { "Comment History", "Add Comment History respons returned null." } };
                    return ApiResponse<CommentDTO>.Failed(error, "Error When adding Comment History.");
                }
                #endregion

                #region request model mapping
                existingComment.CommentDescription = dto.CommentDescription;
                existingComment.UpdatedAt = DateTime.Now;
                #endregion

                var result = await _commentRepository.Update(existingComment);
                if (result != null)
                {
                    #region response model mapping
                    var response = new CommentDTO
                    {
                        CommentId = result.CommentId,
                        BlogId = result.BlogId,
                        User = new CommentUser
                        {
                            UserId = result.User.Id,
                            Name = result.User.UserName
                        },
                        CommentDescription = result.CommentDescription,
                        CreatedAt = result.CreatedAt,
                        UpdatedAt = result.UpdatedAt
                    };
                    #endregion
                    return ApiResponse<CommentDTO>.Success(response, "Comment Updated Successfully.");
                }
            }
            var errors = new Dictionary<string, string>() { { "Comment", "Comment Not Found." } };
            return ApiResponse<CommentDTO>.Failed(errors, "Comment Update Failed");
        }

        public async Task<ApiResponse<string>> DeleteComment(int commentId, string userId)
        {
            var comment = await _commentRepository.GetById(commentId);
            if (comment != null)
            {
                if (comment.UserId != userId)
                {
                    var authError = new Dictionary<string, string>() { { "Authorization", "You are not authorized to delete this comment." } };
                    return ApiResponse<string>.Failed(authError, "Unauthorized blog deletion attempt.");
                }
                try
                {
                    // Checking if the comment is already deleted or not
                    if (comment.IsDeleted == true)
                    {
                        var blogError = new Dictionary<string, string>() { { "Comment", "Comment is already softdeleted." } };
                        return ApiResponse<string>.Failed(blogError, "Comment Deletion Failed");
                    }
                    comment.IsDeleted = true;
                    await _commentRepository.Update(comment);
                    await _blogService.UpdateBlogCommentCount(comment.BlogId, false); // Decrease comment count
                    return ApiResponse<string>.Success(null, "Comment Deleted Successfully");
                }
                catch (Exception ex)
                {
                    var exceptionErrors = new Dictionary<string, string>() { { "Exception", ex.Message } };
                    return ApiResponse<string>.Failed(exceptionErrors, "Comment Deletion Failed Due to an Error");
                }
            }
            var errors = new Dictionary<string, string>() { { "Comment", "Comment Not Found." } };
            return ApiResponse<string>.Failed(errors, "Comment Deletion failed.");
        }

        public async Task UpdateCommentVoteCount(AddOrUpdateCommentReactionDTO model, bool reactionExists, VoteType? previousVote)
        {
            var comment = await _commentRepository.GetById(model.CommentId);
            if (comment != null)
            {
                if (!reactionExists) // New reaction
                {
                    if (model.ReactionType == VoteType.UpVote)
                        comment.UpVoteCount++;
                    else if (model.ReactionType == VoteType.DownVote)
                        comment.DownVoteCount++;
                }
                else // Reaction is being updated
                {
                    // Remove previous vote
                    if (previousVote == VoteType.UpVote)
                        comment.UpVoteCount--;
                    else if (previousVote == VoteType.DownVote)
                        comment.DownVoteCount--;

                    // Apply new vote
                    if (model.ReactionType == VoteType.UpVote)
                        comment.UpVoteCount++;
                    else if (model.ReactionType == VoteType.DownVote)
                        comment.DownVoteCount++;
                }

                // Ensure no negative votes
                comment.UpVoteCount = Math.Max(0, comment.UpVoteCount);
                comment.DownVoteCount = Math.Max(0, comment.DownVoteCount);

                await _commentRepository.Update(comment);
            }
        }
    }
}
