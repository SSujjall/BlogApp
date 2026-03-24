using BlogApp.Application.DTOs.PaymentDTOs;
using BlogApp.Application.Helpers.HelperModels;

namespace BlogApp.Application.Interface.IServices.IPaymentService
{
    public interface IPaymentOrchestrationService
    {
        Task<ApiResponse<bool>> HandlePaymentVerification(string userId, VerifyPaymentDTO dto);
        Task<ApiResponse<bool>> HandleOrderCancellation(string userId, int orderId);
    }
}
