using AutoMapper;
using BlogApp.Application.DTOs;
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
            if (orderRes.Data is null)
            {
                return ApiResponse<string>.Failed(
                    new() { { "OrderNotFound", $"No order found" } },
                    "Payment initiation failed",
                    HttpStatusCode.BadRequest
                );
            }

            #region Creating payment data and saving to database
            try
            {
                var paymentModel = _mapper.Map<Payments>(dto);
                paymentModel.UserId = userId;
                paymentModel.Amount = orderRes.Data.Amount;
                paymentModel.OrderId = dto.OrderId;

                var x = await _paymentRepo.AddAsync(paymentModel);
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

            var paymentProvider = _paymentFactory.GetPaymentProvider(dto.Provider);

            #region payment req dto model mapping
            var paymentReq = new PaymentRequestDTO
            {
                UserId = userId,
                SubscriptionId = orderRes.Data.SubscriptionId,
                SubscriptionName = orderRes.Data.Subscription.Name,
                OrderId = dto.OrderId,
                TotalAmount = orderRes.Data.Amount
            };
            #endregion

            var paymentResult = await paymentProvider.ProcessPaymentAsync(paymentReq);
            if (string.IsNullOrEmpty(paymentResult))
            {
                return ApiResponse<string>.Failed(
                    new() { { "Payment", "Failed to process payment" } },
                    "Payment initiation failed"
                );
            }
            return ApiResponse<string>.Success(
                paymentResult,
                "Payment initiated successfully",
                HttpStatusCode.OK
            );
        }

        public async Task<ApiResponse<bool>> VerifyPayment(string userId, VerifyPaymentDTO dto)
        {
            return await _txnService.ExecuteInTransactionAsync(async () =>
            {
                var paymentProvider = _paymentFactory.GetPaymentProvider(dto.Provider);
                var verificationResult = await paymentProvider.VerifyPaymentAsync(dto.Data);

                if (!verificationResult.IsSuccess)
                {
                    return ApiResponse<bool>.Failed(
                        new() { { "PaymentVerification", "Payment verification failed" } },
                        "Payment verification failed",
                        HttpStatusCode.BadRequest
                    );
                }

                var payment = await _paymentRepo.FindSingleByConditionAsync(
                    x => x.ExternalTransactionId == verificationResult.TransactionUuid && x.UserId == userId
                );
                if (payment is null)
                {
                    throw new ServiceException(
                        new() { { "PaymentNotFound", "Payment record not found" } },
                        HttpStatusCode.NotFound
                    );
                }

                payment.Status = PaymentStatus.Success;
                //await _paymentRepo.SaveChangesAsync(); // No need to call SaveChangesAsync here, it will be called at the end of the transaction in the transaction service

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

        public Task<bool> RefundPayment()
        {
            throw new NotImplementedException();
        }
    }
}
