using BlogApp.Application.DTOs.PaymentDTOs;
using BlogApp.Domain.Entities;

namespace BlogApp.Application.Interface.IServices.IPaymentService
{
    public interface IPaymentProvider
    {
        Task<string> ProcessPaymentAsync(PaymentRequestDTO dto);
        Task<PaymentVerificationResponseDTO> VerifyPaymentAsync(string data);
        Task<object> CheckStatusAsync(Payments payment);
        Task<bool> RefundPaymentAsync(string transactionId);
    }
}
