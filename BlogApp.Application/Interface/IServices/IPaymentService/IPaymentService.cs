using BlogApp.Application.DTOs.PaymentDTOs;
using BlogApp.Application.Helpers.HelperModels;
using BlogApp.Domain.Entities;
using BlogApp.Domain.Enums;

namespace BlogApp.Application.Interface.IServices.IPaymentService
{
    public interface IPaymentService
    {
        Task<ApiResponse<PaymentInitiateResponseDTO>> InitiatePayment(string userId, CreatePaymentDTO dto);

        // Returns internal result — orchestrator handles cross-domain side effects
        Task<InternalPaymentVerificationResultDTO> VerifyPayment(string userId, VerifyPaymentDTO dto);
        
        Task<ApiResponse<object>> CheckPaymentStatus(int paymentId);
        Task<ApiResponse<PaymentInitiateResponseDTO>> RetryPayment(string userId, int paymentId);
        Task<bool> RefundPayment();
        Task<ApiResponse<IEnumerable<Payments>>> GetAllUserPayments(string userId);
        Task<ApiResponse<Payments>> GetPaymentById(string userId, int paymentId);
        Task<ApiResponse<IEnumerable<Payments>>> GetPaymentsOfAnOrder(string userId, int orderId);
        Task CancelPaymentsForOrder(string userId, int orderId);
    }
}
