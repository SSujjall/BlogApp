using BlogApp.Application.DTOs;
using BlogApp.Application.Interface.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentReactionController : ControllerBase
    {
        private readonly ICommentReactionService _commentReactionService;
        public CommentReactionController(ICommentReactionService commentReactionService)
        {
            _commentReactionService = commentReactionService;
        }

        [HttpGet("get-all/{commentId}")]
        public async Task<IActionResult> GetAllReactions(int commentId)
        {
            var response = await _commentReactionService.GetAllCommentVotes(commentId);
            if (response.Status == false)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpPost("vote")]
        public async Task<IActionResult> VoteComment(AddOrUpdateCommentReactionDTO model)
        {
            var userId = User?.FindFirst("UserId")?.Value;
            if (userId == null)
            {
                return Unauthorized("User invalid");
            }

            var response = await _commentReactionService.VoteComment(model, userId);
            if (response.Status == false)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpGet("get-by-id/{commentId}")]
        public async Task<IActionResult> GetById(int commentId)
        {
            //var userId = User?.FindFirst("UserId")?.Value;
            //if (userId == null)
            //{
            //    return Unauthorized("User invalid");
            //}

            var response = await _commentReactionService.GetCommentVoteById(commentId);
            if (response.Status == false)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpGet("get-all-user-comment-reactions")]
        public async Task<IActionResult> GetAllUserCommentReaction()
        {
            var userId = User?.FindFirst("UserId")?.Value;
            if (userId == null)
            {
                return Unauthorized("User invalid");
            }

            var response = await _commentReactionService.GetAllUserCommentReactions(userId);
            if (response.Status == false)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }
    }
}
