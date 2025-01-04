using Azure;
using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers;
using BlogApp.Application.Interface.IRepositories;
using BlogApp.Application.Interface.IServices;
using BlogApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Infrastructure;
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
        public async Task<ApiResponse<CommentDTO>> AddComment(AddCommentDTO dto)
        {
            #region request model mapping
            var request = new Comments
            {
                BlogId = dto.BlogId,
                UserId = dto.UserId,
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

        public async Task<ApiResponse<IEnumerable<CommentDTO>>> GetAllComment(int blogId)
        {
            var requestFilter = new GetRequest<Comments>
            {
                Filter = x => x.BlogId == blogId
            };

            var result = await _commentRepository.GetAll(requestFilter);

            if (result != null)
            {
                #region response model mapping (using EF instead of normal foreach loop)
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
    }
}
