using BlogApp.Domain.Enums;
using System.Text.Json.Serialization;

namespace BlogApp.Application.DTOs.PaymentDTOs
{
    public class CreatePaymentDTO
    {
        public int OrderId { get; set; }
        public PaymentProviderType Provider { get; set; }
    }

    public class PaymentInitiateResponseDTO
    {
        public string RedirectUrl { get; set; }
        public string ExternalTxnId { get; set; }

        [JsonIgnore]
        public string RawResponse { get; set; }
    }

    public class VerifyPaymentDTO
    {
        public string ExternalTxnId { get; set; }
        public string? Data { get; set; }
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
        public string ExternalTxnId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string RawResponse { get; set; }
    }

    public class PaymentCheckStatusResponseDTO
    {
        public string ExternalTxnId { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public string RefTxnId { get; set; }
    }

    public class InternalPaymentVerificationResultDTO
    {
        public bool AlreadyVerified { get; set; }
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public int SubscriptionId { get; set; }
    }
}
