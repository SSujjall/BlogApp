using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers.HelperModels;
using BlogApp.Domain.Enums;

namespace BlogApp.Application.Interface.IServices.IPaymentService
{
    public interface IPaymentService
    {
        Task<ApiResponse<string>> InitiatePayment(string userId, CreatePaymentDTO dto);
        Task<ApiResponse<bool>> VerifyPayment(string userId, VerifyPaymentDTO dto);
        Task<bool> RefundPayment();
    }
}
