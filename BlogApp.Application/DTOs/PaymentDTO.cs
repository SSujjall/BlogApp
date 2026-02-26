using BlogApp.Domain.Enums;

namespace BlogApp.Application.DTOs
{
    public class CreatePaymentDTO
    {
        public PaymentProviderType Provider { get; set; }
    }
}
