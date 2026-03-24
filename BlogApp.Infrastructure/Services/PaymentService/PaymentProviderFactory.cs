using BlogApp.Application.Exceptions;
using BlogApp.Application.Interface.IServices.IPaymentService;
using BlogApp.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace BlogApp.Infrastructure.Services.PaymentService
{
    public class PaymentProviderFactory : IPaymentProviderFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public PaymentProviderFactory(IServiceProvider serviceProvicer)
        {
            _serviceProvider = serviceProvicer;
        }

        public IPaymentProvider GetPaymentProvider(PaymentProviderType providerType)
        {
            return providerType switch
            {
                PaymentProviderType.Esewa => _serviceProvider.GetRequiredService<EsewaPaymentService>(),
                PaymentProviderType.Khalti => _serviceProvider.GetRequiredService<KhaltiPaymentService>(),
                _ => throw new ServiceException(
                    new Dictionary<string, string> { { "PaymentError", $"Payment provider '{providerType}' is not supported." } },
                    HttpStatusCode.BadGateway)
            };
        }
    }
}
