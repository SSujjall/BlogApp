using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Domain.Entities
{
    public class PaymentLogs
    {
        [Key]
        public int LogId { get; set; }

        [Required]
        [ForeignKey(nameof(Payment))]
        public int PaymentId { get; set; }

        public string? RequestPayload { get; set; }
        public string? ResponsePayload { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Property
        public virtual Payments Payment { get; set; } = new Payments();
    }
}
