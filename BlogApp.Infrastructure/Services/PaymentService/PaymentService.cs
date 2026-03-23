using AutoMapper;
using BlogApp.Application.DTOs;
using BlogApp.Application.DTOs.PaymentDTOs;
using BlogApp.Application.Exceptions;
using BlogApp.Application.Helpers.HelperModels;
using BlogApp.Application.Interface.IRepositories;
using BlogApp.Application.Interface.IServices;
using BlogApp.Application.Interface.IServices.IPaymentService;
using BlogApp.Domain.Entities;
using BlogApp.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BlogApp.Infrastructure.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepo;
        private readonly IPaymentProviderFactory _paymentFactory;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly ITransactionService _txnService;

        public PaymentService(
            IPaymentRepository paymentRepo,
            IPaymentProviderFactory paymentFactory,
            IOrderService orderService,
            IMapper mapper,
            ITransactionService txnService
        )
        {
            _paymentRepo = paymentRepo;
            _paymentFactory = paymentFactory;
            _orderService = orderService;
            _mapper = mapper;
            _txnService = txnService;
        }

        public async Task<ApiResponse<string>> InitiatePayment(string userId, CreatePaymentDTO dto)
        {
            var orderRes = await _orderService.GetOrderById(userId, dto.OrderId);
            if (orderRes.Data is null || orderRes.Data.Status == OrderStatus.Completed)
            {
                return ApiResponse<string>.Failed(
                    new() { { "OrderError", $"Order not found or already completed" } },
                    "Payment initiation failed",
                    HttpStatusCode.BadRequest
                );
            }

            var paymentProvider = _paymentFactory.GetPaymentProvider(dto.Provider);

            #region payment req dto model mapping
            var paymentReq = new PaymentRequestDTO
            {
                UserId = userId,
                SubscriptionId = orderRes.Data.SubscriptionId,
                SubscriptionName = orderRes.Data.Subscription.Name,
                OrderId = dto.OrderId,
                TotalAmount = orderRes.Data.Amount,
                ExternalTransactionId = Guid.NewGuid().ToString(),
            };
            #endregion

            var paymentResult = await paymentProvider.ProcessPaymentAsync(paymentReq);
            if (paymentResult == null || string.IsNullOrEmpty(paymentResult.RedirectUrl))
            {
                return ApiResponse<string>.Failed(
                    new() { { "Payment", "Failed to process payment" } },
                    "Payment initiation failed"
                );
            }

            #region Creating payment data and saving to database
            try
            {
                var paymentModel = _mapper.Map<Payments>(dto);
                paymentModel.UserId = userId;
                paymentModel.Amount = orderRes.Data.Amount;
                paymentModel.OrderId = dto.OrderId;

                // use the externalTxnId from the returned result from payment
                paymentModel.ExternalTransactionId = paymentResult.ExternalTxnId;

                await _paymentRepo.AddAsync(paymentModel);
                await _paymentRepo.SaveChangesAsync();
            }
            catch (DbUpdateException) // Only catch database exceptions, let the global excception handler catch other exceptions
            {
                throw new ServiceException(
                    new() { { "PaymentCreationError", "Unable to create payment." } },
                    HttpStatusCode.InternalServerError
                );
            }
            #endregion

            return ApiResponse<string>.Success(
                paymentResult.RedirectUrl,
                "Payment initiated successfully",
                HttpStatusCode.OK
            );
        }

        public async Task<ApiResponse<bool>> VerifyPayment(string userId, VerifyPaymentDTO dto)
        {
            return await _txnService.ExecuteInTransactionAsync(async () =>
            {
                var payment = await _paymentRepo.FindSingleByConditionAsync(
                    x => x.ExternalTransactionId == dto.ExternalTxnId && x.UserId == userId
                );
                if (payment is null)
                {
                    throw new ServiceException(
                        new() { { "PaymentNotFound", "Payment record not found" } },
                        HttpStatusCode.NotFound
                    );
                }
                if (payment.Status == PaymentStatus.Success)
                {
                    return ApiResponse<bool>.Success(true, "Payment already verified", HttpStatusCode.OK);
                }

                var paymentProvider = _paymentFactory.GetPaymentProvider(payment.Provider);
                var verificationResult = await paymentProvider.VerifyPaymentAsync(dto.Data);

                if (!verificationResult.IsSuccess)
                {
                    return ApiResponse<bool>.Failed(
                        new() { { "PaymentVerification", "Payment verification failed" } },
                        "Payment verification failed",
                        HttpStatusCode.BadRequest
                    );
                }
                if (verificationResult.ExternalTxnId != payment.ExternalTransactionId)
                {
                    throw new ServiceException(
                        new() { { "TransactionMismatch", "Transaction mismatch" } },
                        HttpStatusCode.BadRequest
                    );
                }

                payment.Status = PaymentStatus.Success;
                //await _paymentRepo.SaveChangesAsync(); // No need to call SaveChangesAsync here, it will be called at the end of the transaction in the transaction service

                // Check if order is already fullfilled, if yes then throw error
                var order = await _orderService.GetOrderById(userId, payment.OrderId);
                if (order.Data.Status == OrderStatus.Completed)
                {
                    throw new ServiceException(
                        new() { { "OrderStatusError", "Order already marked as completed and verified" } },
                        HttpStatusCode.BadRequest
                    );
                }

                var updateOrderModel = new UpdateOrderDTO
                {
                    OrderId = payment.OrderId,
                    Status = OrderStatus.Completed,
                };
                var updatedOrder = await _orderService.UpdateOrder(userId, updateOrderModel);
                if (updatedOrder.StatusCode != HttpStatusCode.OK)
                {
                    throw new ServiceException(
                        updatedOrder.Errors,
                        HttpStatusCode.BadRequest
                    );
                }

                return ApiResponse<bool>.Success(
                    true,
                    "Payment verified and order updated successfully",
                    HttpStatusCode.OK
                );
            });
        }

        public async Task<ApiResponse<object>> CheckPaymentStatus(int paymentId)
        {
            var payment = await _paymentRepo.GetByIdAsync(paymentId);
            if (payment is null)
            {
                throw new ServiceException(
                    new() { { "PaymentNotFound", "Payment record not found" } },
                    HttpStatusCode.NotFound
                );
            }
            var paymentProvider = _paymentFactory.GetPaymentProvider(payment.Provider);
            var result = await paymentProvider.CheckStatusAsync(payment);
            return ApiResponse<object>.Success(result, "Status fetched successfully");
        }

        public Task<bool> RefundPayment()
        {
            throw new NotImplementedException();
        }
    }
}
