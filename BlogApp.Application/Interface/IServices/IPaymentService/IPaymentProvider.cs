using BlogApp.Application.DTOs;

namespace BlogApp.Application.Interface.IServices.IPaymentService
{
    public interface IPaymentProvider
    {
        Task<string> ProcessPaymentAsync(PaymentRequestDTO dto);
        Task<PaymentVerificationResponseDTO> VerifyPaymentAsync(string data);
        Task<object> StatusCheckAsync(string transactionId);
        Task<bool> RefundPaymentAsync(string transactionId);
    }
}
