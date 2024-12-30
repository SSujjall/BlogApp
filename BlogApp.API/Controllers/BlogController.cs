﻿using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers;
using BlogApp.Application.Interface.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BlogApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [AllowAnonymous]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllBlogs()
        {
            var response = await _blogService.GetAllBlogs();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return StatusCode((int)response.StatusCode, response);
            }

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("get-blog/{id}")]
        public async Task<IActionResult> GetBlog(int id)
        {
            var response = await _blogService.GetBlogById(id);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return StatusCode((int)response.StatusCode, response);
            }

            return Ok(response);
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateBlog(CreateBlogDTO dto)
        {
            var userId = User.FindFirst("UserId")?.Value;

            var response = await _blogService.CreateBlog(userId, dto);
            if (response.Status != true)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpPost("update")]
        public Task<IActionResult> UpdateBlog()
        {
            return null;
        }

        [Authorize]
        [HttpPost("delete/{id}")]
        public Task<IActionResult> DeleteBlog(int id)
        {
            return null;
        }
    }
}
