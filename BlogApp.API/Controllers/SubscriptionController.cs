using BlogApp.Application.DTOs;
using BlogApp.Application.Interface.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BlogApp.API.Controllers
{
    [Authorize(Roles = "Superadmin")]
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController(
        ISubscriptionService _subsService
    ) : ControllerBase
    {
        [HttpPost("create")]
        public async Task<IActionResult> CreateSubscription(CreateSubscriptionDTO req)
        {
            var response = await _subsService.CreateNewSubscription(req);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllSubscriptions()
        {
            var response = await _subsService.GetAllSubscriptions();
            return Ok(response);
        }

        [HttpPatch("update")]
        public async Task<IActionResult> UpdateSubscription(UpdateSubscriptionDTO req)
        {
            var response = await _subsService.UpdateSubscription(req);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteSubscription(int id)
        {
            var response = await _subsService.DeleteSubscription(id);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }
    }
}
