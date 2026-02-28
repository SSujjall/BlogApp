using BlogApp.Application.DTOs;
using BlogApp.Application.Interface.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BlogApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController(
        ISubscriptionService _subsService
    ) : ControllerBase
    {
        [Authorize(Roles = "Superadmin, Admin")]
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
    }
}
