using BlogApp.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Domain.Entities
{
    public class Payments
    {
        [Key]
        public int PaymentId { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }

        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public PaymentProviderType Provider { get; set; }
        public string? ExternalTransactionId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Property
        public virtual Users User { get; set; } = new Users();
        public virtual Orders Order { get; set; } = new Orders();
        public virtual ICollection<Refunds> Refunds { get; set; } = new List<Refunds>();
        public virtual ICollection<PaymentLogs> PaymentLogs { get; set; } = new List<PaymentLogs>();
    }
}
