using BlogApp.Domain.Enums;

namespace BlogApp.Application.DTOs
{
    public class CreatePaymentDTO
    {
        public int OrderId { get; set; }
        public PaymentProviderType Provider { get; set; }
    }

    public class PaymentRequestDTO
    {
        public string UserId { get; set; }
        public int SubscriptionId { get; set; }
        public string SubscriptionName { get; set; }
        public int OrderId { get; set; }
        public decimal TotalAmount { get; set; }

    }
}
