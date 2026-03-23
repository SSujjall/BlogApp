using BlogApp.Application.DTOs.PaymentDTOs;
using BlogApp.Application.Helpers.HelperModels;
using BlogApp.Domain.Enums;

namespace BlogApp.Application.Interface.IServices.IPaymentService
{
    public interface IPaymentService
    {
        Task<ApiResponse<PaymentInitiateResponseDTO>> InitiatePayment(string userId, CreatePaymentDTO dto);
        Task<ApiResponse<bool>> VerifyPayment(string userId, VerifyPaymentDTO dto);
        Task<ApiResponse<object>> CheckPaymentStatus(int paymentId);
        Task<bool> RefundPayment();
    }
}
