using BlogApp.Application.DTOs.PaymentDTOs;
using BlogApp.Domain.Entities;

namespace BlogApp.Application.Interface.IServices.IPaymentService
{
    public interface IPaymentProvider
    {
        Task<PaymentInitiateResponseDTO> ProcessPaymentAsync(PaymentRequestDTO dto);
        Task<PaymentVerificationResponseDTO> VerifyPaymentAsync(VerifyPaymentDTO dto);
        Task<PaymentCheckStatusResponseDTO> CheckStatusAsync(Payments payment);
        Task<bool> RefundPaymentAsync(string transactionId);
    }
}
