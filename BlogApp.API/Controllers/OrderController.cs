using BlogApp.Application.DTOs;
using BlogApp.Application.Exceptions;
using BlogApp.Application.Interface.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BlogApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(
        IOrderService _orderService
    ) : ControllerBase
    {
        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDTO reqModel)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                throw new ServiceException(new() { { "Unauthorized", "User not authorized" } }, HttpStatusCode.Unauthorized);
            }
            var response = await _orderService.CreateNewOrder(userId, reqModel);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Superadmin,Admin")]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllOrders()
        {
            var response = await _orderService.GetAllOrders();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpGet("get-user-orders")]
        public async Task<IActionResult> GetUserOrders()
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                throw new ServiceException(new() { { "Unauthorized", "User not authorized" } }, HttpStatusCode.Unauthorized);
            }
            var response = await _orderService.GetOrdersByUserId(userId);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                throw new ServiceException(new() { { "Unauthorized", "User not authorized" } }, HttpStatusCode.Unauthorized);
            }
            var response = await _orderService.GetOrderById(userId, id);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }
    }
}
