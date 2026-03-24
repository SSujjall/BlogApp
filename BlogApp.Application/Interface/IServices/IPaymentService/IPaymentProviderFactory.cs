using BlogApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogApp.Application.Interface.IServices.IPaymentService
{
    public interface IPaymentProviderFactory
    {
        IPaymentProvider GetPaymentProvider(PaymentProviderType providerType);
    }
}
