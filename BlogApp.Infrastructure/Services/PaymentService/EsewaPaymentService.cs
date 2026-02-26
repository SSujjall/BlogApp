using BlogApp.Application.Interface.IServices.IPaymentService;
using BlogApp.Domain.Entities;

namespace BlogApp.Infrastructure.Services.PaymentService
{
    public class EsewaPaymentService : IPaymentProvider
    {
        public Task<string> ProcessPaymentAsync(Payments payment)
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
