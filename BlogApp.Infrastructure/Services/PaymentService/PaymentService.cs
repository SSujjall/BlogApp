using BlogApp.Application.DTOs;
using BlogApp.Application.Helpers.HelperModels;
using BlogApp.Application.Interface.IServices.IPaymentService;

namespace BlogApp.Infrastructure.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentProviderFactory _paymentFactory;

        public PaymentService(IPaymentProviderFactory paymentFactory)
        {
            _paymentFactory = paymentFactory;
        }

        public Task<ApiResponse<string>> InitiatePayment(CreatePaymentDTO dto)
        {
            var paymentProvider = _paymentFactory.GetPaymentProvider(dto.Provider);
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
