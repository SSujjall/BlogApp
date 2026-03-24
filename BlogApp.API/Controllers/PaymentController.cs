using BlogApp.Application.DTOs.PaymentDTOs;
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
        private readonly IPaymentOrchestrationService _orchestrationService;

        public PaymentController(
            IPaymentService paymentService,
            IPaymentOrchestrationService orchestrationService
        )
        {
            _paymentService = paymentService;
            _orchestrationService = orchestrationService;
        }

        [Authorize]
        [HttpPost("initiate")]
        public async Task<IActionResult> ProcessPayment([FromBody] CreatePaymentDTO reqModel)
        {
            var userId = GetUserId();
            var response = await _paymentService.InitiatePayment(userId, reqModel);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpPost("verify")]
        public async Task<IActionResult> VerifyPayment([FromBody] VerifyPaymentDTO reqModel)
        {
            var userId = GetUserId();

            // Instead of payment service, orchestrator service is called
            // for proper verification orchestration and table updates for order, user and payment
            var response = await _orchestrationService.HandlePaymentVerification(userId, reqModel);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("check-status/{paymentId}")]
        public async Task<IActionResult> StatusCheck(int paymentId)
        {
            var userId = GetUserId();
            var response = await _paymentService.CheckPaymentStatus(paymentId);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpGet("retry/{paymentId}")]
        public async Task<IActionResult> RetryPayment(int paymentId)
        {
            var userId = GetUserId();
            var response = await _paymentService.RetryPayment(userId, paymentId);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpGet("get-all/{orderId}")]
        public async Task<IActionResult> GetPaymentsByOrderId(int orderId)
        {
            var userId = GetUserId();
            var response = await _paymentService.GetPaymentsOfAnOrder(userId, orderId);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return StatusCode((int)response.StatusCode, response);
            }
            return Ok(response);
        }

        private string GetUserId()
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                throw new ServiceException(new() { { "Unauthorized", "User not authorized" } }, HttpStatusCode.Unauthorized);
            }
            return userId;
        }
    }
}
