using Azure;
using BlogApp.Application.DTOs;
using BlogApp.Application.Exceptions;
using BlogApp.Application.Interface.IServices.IPaymentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BlogApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("initiate")]
        [Authorize]
        public async Task<IActionResult> ProcessPayment([FromBody] CreatePaymentDTO reqModel)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                throw new ServiceException(new() { { "Unauthorized", "User not authorized" } }, HttpStatusCode.Unauthorized);
            }

            var response = await _paymentService.InitiatePayment(userId, reqModel);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new ServiceException(
                    new Dictionary<string, string> { { "Payment", "Failed to initiate payment" } },
                    HttpStatusCode.InternalServerError);
            }
            return Ok(response);
        }

        [HttpPost("verify")]
        [Authorize]
        public async Task<IActionResult> VerifyPayment([FromBody] VerifyPaymentDTO reqModel)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                throw new ServiceException(new() { { "Unauthorized", "User not authorized" } }, HttpStatusCode.Unauthorized);
            }

            var response = await _paymentService.VerifyPayment(userId, reqModel);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new ServiceException(
                    new Dictionary<string, string> { { "Payment", "Failed to initiate payment" } },
                    HttpStatusCode.InternalServerError);
            }
            return Ok(response);
        }
    }
}
