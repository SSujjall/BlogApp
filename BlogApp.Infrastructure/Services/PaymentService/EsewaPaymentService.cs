using BlogApp.Application.DTOs;
using BlogApp.Application.Interface.IServices.IPaymentService;

namespace BlogApp.Infrastructure.Services.PaymentService
{
    public class EsewaPaymentService : IPaymentProvider
    {
        public Task<string> ProcessPaymentAsync(CreatePaymentDTO payment)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RefundPaymentAsync(string transactionId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> VerifyPaymentAsync(string transactionId)
        {
            throw new NotImplementedException();
        }
    }
}
