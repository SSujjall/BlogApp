using BlogApp.Domain.Entities;

namespace BlogApp.Application.Interface.IServices.IPaymentService
{
    public interface IPaymentProvider
    {
        Task<string> ProcessPaymentAsync(Payments payment);
        Task<bool> VerifyPaymentAsync(string transactionId);
        Task<bool> RefundPaymentAsync(string transactionId);
    }
}
