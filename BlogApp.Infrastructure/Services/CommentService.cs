using Azure;
using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers;
using BlogApp.Application.Helpers.HelperModels;
using BlogApp.Application.Interface.IRepositories;
using BlogApp.Application.Interface.IServices;
using BlogApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Infrastructure.Services
{
    public class CommentService(ICommentRepository _commentRepository) : ICommentService
    {
        public async Task<ApiResponse<IEnumerable<CommentDTO>>> GetAllCommentByBlogId(int blogId)
        {
            var requestFilter = new GetRequest<Comments>
            {
                Filter = (x => x.BlogId == blogId)
            };

            var result = await _commentRepository.GetAll(requestFilter);
            if (result != null)
            {
                #region response model mapping (using LINQ instead of normal foreach loop)
                var response = result.Select(comment => new CommentDTO
                {
                    CommendId = comment.CommendId,
                    BlogId = comment.BlogId,
                    UserId = comment.UserId,
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
                #region response model mapping
                var response = new CommentDTO
                {
                    CommendId = result.CommendId,
                    BlogId = result.BlogId,
                    UserId = result.UserId,
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
            var comment = await _commentRepository.GetById(dto.CommendId);
            if (comment != null)
            {
                if (comment.UserId != userId)
                {
                    var authError = new Dictionary<string, string>() { { "Authorization", "You are not authorized to update this comment." } };
                    return ApiResponse<CommentDTO>.Failed(authError, "Unauthorized blog update attempt.");
                }

                #region request model mapping
                comment.CommentDescription = dto.CommentDescription;
                comment.UpdatedAt = DateTime.Now;
                #endregion

                var result = await _commentRepository.Update(comment);
                if (result != null)
                {
                    #region response model mapping
                    var response = new CommentDTO
                    {
                        CommendId = result.CommendId,
                        BlogId = result.BlogId,
                        UserId = result.UserId,
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
                    comment.IsDeleted = true;
                    await _commentRepository.Update(comment);
                    return ApiResponse<string>.Success(null, "Comment Deleted Successfully");
                }
                catch (Exception ex)
                {
                    var exceptionErrors = new Dictionary<string, string>() { { "Exception", ex.Message } };
                    return ApiResponse<string>.Failed(exceptionErrors, "Comment Deletion Failed Due to an Error");
                }
            }
            var errors = new Dictionary<string, string>() { { "Comment", "Comment Not Found." } };
            return ApiResponse<string>.Failed(errors, "Comment Deleteion failed.");
        }
    }
}
