using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers.HelperModels;
using BlogApp.Application.Interface.IServices.IPaymentService;

namespace BlogApp.Infrastructure.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        public Task<ApiResponse<string>> InitiatePayment(CreatePaymentDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RefundPayment()
        {
            throw new NotImplementedException();
        }

        public Task<bool> VerifyPayment()
        {
            throw new NotImplementedException();
        }
    }
}
