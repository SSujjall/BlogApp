using BlogApp.Application.DTOs.PaymentDTOs;
using BlogApp.Application.Interface.IServices.IPaymentService;
using BlogApp.Domain.Entities;

namespace BlogApp.Infrastructure.Services.PaymentService
{
    public class KhaltiPaymentService : IPaymentProvider
    {
        public Task<string> ProcessPaymentAsync(PaymentRequestDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<PaymentVerificationResponseDTO> VerifyPaymentAsync(string data)
        {
            throw new NotImplementedException();
        }

        public Task<object> CheckStatusAsync(Payments payment)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RefundPaymentAsync(string transactionId)
        {
            throw new NotImplementedException();
        }
    }
}
