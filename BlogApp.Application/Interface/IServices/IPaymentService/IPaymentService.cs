using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers.HelperModels;

namespace BlogApp.Application.Interface.IServices.IPaymentService
{
    public interface IPaymentService
    {
        Task<ApiResponse<string>> InitiatePayment(string userId, CreatePaymentDTO dto);
        Task<bool> VerifyPayment();
        Task<bool> RefundPayment();
    }
}
