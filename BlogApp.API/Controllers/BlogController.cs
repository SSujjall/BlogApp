﻿using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers;
using BlogApp.Application.Helpers.HelperModels;
using BlogApp.Application.Interface.IServices;
using BlogApp.Domain.Entities;
using BlogApp.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BlogApp.API.Controllers
{
    [EnableRateLimiting("WritePolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [EnableRateLimiting("ReadPolicy")]
        [AllowAnonymous]
        [HttpGet("get-blogs")]
        public async Task<IActionResult> GetBlogs([FromQuery] string? sortBy, [FromQuery] int? skip, [FromQuery] int? take, [FromQuery] string? search)
        {
            var request = new GetRequest<Blogs>
            {
                Filter = !string.IsNullOrEmpty(search) ? b => EF.Functions.Like(b.Title.ToLower(), $"%{search.ToLower()}%") || EF.Functions.Like(b.Description.ToLower(), $"%{search.ToLower()}%") : null,
                Skip = skip ?? 0,
                Take = take ?? 10,
                SortBy = sortBy?.ToLower() switch
                {
                    "popularity" => "popularity",
                    "recency" => "recency",
                    "random" => "random",
                    _ => "default", // Ensure unrecognized values don't cause issues
                }
            };

            var requestForCache = new CacheRequestItems
            {
                Filter = !string.IsNullOrEmpty(search) ? search.ToLower() : "",
                //OrderBy = request.Filter?.ToString() ?? "null",
                Skip = (skip ?? 0).ToString(),
                Take = (take ?? 10).ToString(),
                SortBy = !string.IsNullOrEmpty(sortBy) ? sortBy.ToLower() : "",
            };

            var response = await _blogService.GetAllBlogs(request, requestForCache);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return StatusCode((int)response.StatusCode, response);
            }

            return Ok(response);
        }

        [EnableRateLimiting("ReadPolicy")]
        [AllowAnonymous]
        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetBlog(int id)
        {
            var response = await _blogService.GetBlogById(id);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return StatusCode((int)response.StatusCode, response);
            }

            return Ok(response);
        }

        [EnableRateLimiting("ReadPolicy")]
        [Authorize]
        [HttpGet("get-user-blogs")]
        public async Task<IActionResult> GetUserBlogs()
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User id not found");
            }

            var response = await _blogService.GetBlogsByUserId(userId);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return Ok(response);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateBlog([FromForm] CreateBlogDTO dto)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User id not found");
            }

            var response = await _blogService.CreateBlog(userId, dto);
            if (response.Status != true)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateBlog([FromForm] UpdateBlogDTO dto)
        {
            var userId = User.FindFirst("UserId").Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResponse<string>.Failed(null, "User not authenticated."));
            }

            // TODO : When updating blog, save the non updated blog to blog history.

            var response = await _blogService.UpdateBlog(dto, userId);
            if (response.Status != true)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var userId = User.FindFirst("UserId").Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(ApiResponse<string>.Failed(null, "User not authenticated."));
            }

            var response = await _blogService.DeleteBlog(id, userId);
            if (response.Status != true)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }
    }
}
