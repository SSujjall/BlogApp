﻿using System.Net;
using BlogApp.Application.DTOs;
using BlogApp.Application.Interface.IServices;
using BlogApp.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController(ICommentService _commentService) : ControllerBase
    {
        [HttpGet("get-every-comment-TEST")]
        public async Task<IActionResult> GetAllComments()
        {
            var response = await _commentService.GetAllComment();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }


        [AllowAnonymous]
        [HttpGet("get-all/{blogId}")]
        public async Task<IActionResult> GetAllComments(int blogId)
        {
            var response = await _commentService.GetAllCommentByBlogId(blogId);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return StatusCode((int)response.StatusCode, response);
            }

            return Ok(response);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateComment(AddCommentDTO dto)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if(userId == null)
            {
                return Unauthorized("User Id Not Found");
            }

            var response = await _commentService.AddComment(dto, userId);
            if (response.Status != true)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateComment(UpdateCommentDTO dto)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (userId == null)
            {
                return Unauthorized("User Id Not Found");
            }

            var response = await _commentService.UpdateComment(dto, userId);
            if (response.Status != true)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User Id Not Found");
            }

            var response = await _commentService.DeleteComment(commentId, userId);
            if (response.Status != true)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }
    }
}
