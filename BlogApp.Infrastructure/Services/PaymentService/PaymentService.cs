using AutoMapper;
using BlogApp.Application.DTOs;
using BlogApp.Application.Exceptions;
using BlogApp.Application.Helpers.HelperModels;
using BlogApp.Application.Interface.IRepositories;
using BlogApp.Application.Interface.IServices;
using BlogApp.Application.Interface.IServices.IPaymentService;
using BlogApp.Domain.Entities;
using CloudinaryDotNet;
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

        public Task<bool> VerifyPayment()
        {
            throw new NotImplementedException();
        }

        public Task<bool> RefundPayment()
        {
            throw new NotImplementedException();
        }
    }
}
