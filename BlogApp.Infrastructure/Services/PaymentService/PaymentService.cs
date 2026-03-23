using AutoMapper;
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
        private readonly IOrderRepository _orderRepo;
        private readonly IMapper _mapper;
        private readonly ITransactionService _txnService;

        public PaymentService(
            IPaymentRepository paymentRepo,
            IPaymentProviderFactory paymentFactory,
            IOrderService orderService,
            IOrderRepository orderRepo,
            IMapper mapper,
            ITransactionService txnService
        )
        {
            _paymentRepo = paymentRepo;
            _paymentFactory = paymentFactory;
            _orderService = orderService;
            _orderRepo = orderRepo;
            _mapper = mapper;
            _txnService = txnService;
        }

        public async Task<ApiResponse<PaymentInitiateResponseDTO>> InitiatePayment(string userId, CreatePaymentDTO dto)
        {
            var orderRes = await _orderService.GetOrderById(userId, dto.OrderId);
            if (orderRes.Data is null)
            {
                return ApiResponse<PaymentInitiateResponseDTO>.Failed(
                    new() { { "OrderError", $"Order not found" } },
                    "Payment initiation failed",
                    HttpStatusCode.BadRequest
                );
            }
            if (orderRes.Data.Status == OrderStatus.Completed)
            {
                return ApiResponse<PaymentInitiateResponseDTO>.Failed(
                    new() { { "OrderError", $"Order already completed" } },
                    "Payment initiation failed",
                    HttpStatusCode.Conflict
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
                return ApiResponse<PaymentInitiateResponseDTO>.Failed(
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
                paymentModel.PaymentUrl = paymentResult.RedirectUrl;

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

            return ApiResponse<PaymentInitiateResponseDTO>.Success(
                paymentResult,
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
                if (payment.Status == PaymentStatus.Canceled || payment.Status == PaymentStatus.Failed)
                {
                    throw new ServiceException(
                        new() { { "PaymentStatusError", "Payment cannot be verified" } }, 
                        HttpStatusCode.BadRequest
                    );
                }

                var paymentProvider = _paymentFactory.GetPaymentProvider(payment.Provider);
                var verificationResult = await paymentProvider.VerifyPaymentAsync(dto);

                if (!verificationResult.IsSuccess)
                {
                    throw new ServiceException(
                        new() { { "PaymentVerification", "Payment verification failed" } },
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
                var order = await _orderRepo.FindSingleByConditionAsync(
                    o => o.OrderId == payment.OrderId && o.UserId == userId
                );
                if (order == null || 
                    order.Status == OrderStatus.Completed ||
                    order.Status == OrderStatus.Canceled
                )
                {
                    throw new ServiceException(
                        new() { { "OrderStatusError", "Order not found or cannot be verified" } },
                        HttpStatusCode.BadRequest
                    );
                }

                // Update order status
                order.Status = OrderStatus.Completed;

                var otherPaymentForSameOrderId = await _paymentRepo.FindAllByConditionAsync(
                    p => p.OrderId == payment.OrderId 
                        && p.PaymentId != payment.PaymentId
                        && p.Status != PaymentStatus.Success
                );

                foreach (var paymentItem in otherPaymentForSameOrderId)
                {
                    paymentItem.Status = PaymentStatus.Canceled;
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
            // TODO: Update the order and payment status according to the check status.
            // Scenario: Failure in payment in our side but is success in the khalti/esewa side, so we need to check status and see if the payment is succeded,
            // if yes then update our DB accordingly
        }

        public Task<bool> RefundPayment()
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<IEnumerable<Payments>>> GetAllUserPayments(string userId)
        {
            var payments = await _paymentRepo.FindAllByConditionAsync(
                x => x.UserId == userId
            );
            if (payments is null)
            {
                throw new ServiceException(
                    new() { { "PaymentNotFound", "Payment records not found" } },
                    HttpStatusCode.NotFound
                );
            }
            return ApiResponse<IEnumerable<Payments>>.Success(payments, "Payments retrieved for user successfully");
        }

        public async Task<ApiResponse<Payments>> GetPaymentById(string userId, int paymentId)
        {
            var payment = await _paymentRepo.FindSingleByConditionAsync(
                x => x.PaymentId == paymentId && x.UserId == userId
            );
            if (payment is null)
            {
                throw new ServiceException(
                    new() { { "PaymentNotFound", "Payment records not found" } },
                    HttpStatusCode.NotFound
                );
            }
            return ApiResponse<Payments>.Success(payment, "Payments retrieved for user successfully");
        }

        public async Task<ApiResponse<IEnumerable<Payments>>> GetPaymentsOfAnOrder(string userId, int orderId)
        {
            var payments = await _paymentRepo.FindAllByConditionAsync(
                x => x.UserId == userId && x.OrderId == orderId
            );
            if (payments is null)
            {
                throw new ServiceException(
                    new() { { "PaymentNotFound", "Payment records not found" } },
                    HttpStatusCode.NotFound
                );
            }
            return ApiResponse<IEnumerable<Payments>>.Success(payments, "Payments retrieved for user successfully for specific order");
        }

        public async Task<ApiResponse<PaymentInitiateResponseDTO>> RetryPayment(string userId, int paymentId)
        {
            var payment = await _paymentRepo.FindSingleByConditionAsync(
                x => x.PaymentId == paymentId && x.UserId == userId
            );
            if (payment is null || (payment.Status != PaymentStatus.Pending && payment.Status != PaymentStatus.Failed))
            {
                throw new ServiceException(
                    new() { { "PaymentRetryError", "Payment records not found or already completed" } },
                    HttpStatusCode.NotFound
                );
            }

            var data = new PaymentInitiateResponseDTO
            {
                RedirectUrl = payment.PaymentUrl,
                ExternalTxnId = payment.ExternalTransactionId,
            };
            return ApiResponse<PaymentInitiateResponseDTO>.Success(
                data,
                "Retry initiated"
            );
        }
    }
}
