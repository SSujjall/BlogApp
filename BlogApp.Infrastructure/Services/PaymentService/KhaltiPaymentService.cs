using BlogApp.Application.DTOs;
using BlogApp.Application.Interface.IServices.IPaymentService;

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

        public Task<object> StatusCheckAsync(string transactionId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RefundPaymentAsync(string transactionId)
        {
            throw new NotImplementedException();
        }
    }
}
