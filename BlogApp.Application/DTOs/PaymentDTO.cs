using BlogApp.Domain.Enums;

namespace BlogApp.Application.DTOs
{
    public class CreatePaymentDTO
    {
        public int OrderId { get; set; }
        public PaymentProviderType Provider { get; set; }
    }

    public class VerifyPaymentDTO
    {
        public PaymentProviderType Provider { get; set; }
        public string Data { get; set; }
    }

    public class PaymentRequestDTO
    {
        public string UserId { get; set; }
        public int SubscriptionId { get; set; }
        public string SubscriptionName { get; set; }
        public int OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public string ExternalTransactionId { get; set; }
    }

    public class PaymentVerificationResponseDTO
    {
        public bool IsSuccess { get; set; }
        public string TransactionId { get; set; }
        public string TransactionUuid { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string RawResponse { get; set; }
    }
}
