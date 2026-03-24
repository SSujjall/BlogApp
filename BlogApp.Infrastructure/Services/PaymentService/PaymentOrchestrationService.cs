using BlogApp.Application.DTOs.PaymentDTOs;
using BlogApp.Application.Helpers.HelperModels;
using BlogApp.Application.Interface.IServices;
using BlogApp.Application.Interface.IServices.IPaymentService;
using System.Net;

namespace BlogApp.Infrastructure.Services.PaymentService
{
    public class PaymentOrchestrationService : IPaymentOrchestrationService
    {
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private readonly ITransactionService _txnService;

        public PaymentOrchestrationService(
            IPaymentService paymentService,
            IOrderService orderService,
            IUserService userService,
            ITransactionService txnService
        )
        {
            _paymentService = paymentService;
            _orderService = orderService;
            _userService = userService;
            _txnService = txnService;
        }

        public async Task<ApiResponse<bool>> HandlePaymentVerification(string userId, VerifyPaymentDTO dto)
        {
            return await _txnService.ExecuteInTransactionAsync(async () =>
            {
                // Step 1: PaymentService does its job — verifies and updates payment records only
                var result = await _paymentService.VerifyPayment(userId, dto);

                // Short-circuit if payment was already verified in a previous call (idempotent)
                if (result.AlreadyVerified)
                {
                    return ApiResponse<bool>.Success(true, "Payment already verified", HttpStatusCode.OK);
                }

                // Step 2: OrderService owns order state — tell it to complete the order
                await _orderService.CompleteOrder(userId, result.OrderId);

                // Step 3: UserService owns subscription state — tell it to update the user
                await _userService.UpdateSubscription(userId, result.SubscriptionId);

                return ApiResponse<bool>.Success(
                    true,
                    "Payment verified and order updated successfully",
                    HttpStatusCode.OK
                );
            });
        }

        public async Task<ApiResponse<bool>> HandleOrderCancellation(string userId, int orderId)
        {
            return await _txnService.ExecuteInTransactionAsync(async () =>
            {
                // Step 1: OrderService cancels the order — owns order state
                var result = await _orderService.CancelOrder(userId, orderId);

                // Step 2: PaymentService cancels related payments — owns payment state
                await _paymentService.CancelPaymentsForOrder(userId, result.OrderId);

                return ApiResponse<bool>.Success(true, "Order canceled successfully", HttpStatusCode.OK);
            });
        }
    }
}
