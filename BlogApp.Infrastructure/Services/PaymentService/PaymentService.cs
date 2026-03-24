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
        private readonly IMapper _mapper;

        public PaymentService(
            IPaymentRepository paymentRepo,
            IPaymentProviderFactory paymentFactory,
            IOrderService orderService,
            IMapper mapper
        )
        {
            _paymentRepo = paymentRepo;
            _paymentFactory = paymentFactory;
            _orderService = orderService;
            _mapper = mapper;
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
            if (orderRes.Data.Status == OrderStatus.Completed || orderRes.Data.Status == OrderStatus.Canceled)
            {
                return ApiResponse<PaymentInitiateResponseDTO>.Failed(
                    new() { { "OrderError", $"Order is no longer payable" } },
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

        public async Task<InternalPaymentVerificationResultDTO> VerifyPayment(string userId, VerifyPaymentDTO dto)
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
                return new InternalPaymentVerificationResultDTO
                {
                    AlreadyVerified = false,
                    OrderId = payment.OrderId,
                    UserId = userId,
                    SubscriptionId = payment.Order.SubscriptionId
                };
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

            payment.Status = PaymentStatus.Success; // Update payment's status

            var otherPaymentForSameOrderId = await _paymentRepo.FindAllByConditionAsync(
                p => p.OrderId == payment.OrderId 
                    && p.PaymentId != payment.PaymentId
                    && p.Status != PaymentStatus.Success
            );

            // cancel the child payments for one order after a successful payment
            foreach (var paymentItem in otherPaymentForSameOrderId)
            {
                paymentItem.Status = PaymentStatus.Canceled;
            }

            return new InternalPaymentVerificationResultDTO
            {
                AlreadyVerified = false,
                OrderId = payment.OrderId,
                UserId = userId,
                SubscriptionId = payment.Order.SubscriptionId 
            };
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

        public async Task CancelPaymentsForOrder(string userId, int orderId)
        {
            var payments = await _paymentRepo.FindAllByConditionAsync(
                x => x.OrderId == orderId
                     && x.UserId == userId
                     && x.Status != PaymentStatus.Success
                     && x.Status != PaymentStatus.Canceled
            );

            foreach (var payment in payments)
            {
                payment.Status = PaymentStatus.Canceled;
            }

            // Note: SaveChangesAsync not called here — orchestrator transaction commits everything
        }
    }
}
