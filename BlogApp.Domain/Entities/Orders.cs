using BlogApp.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BlogApp.Domain.Entities
{
    public class Orders
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        [ForeignKey(nameof(Subscription))]
        public int SubscriptionId { get; set; }

        public decimal Amount { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        [JsonIgnore]
        public virtual Users User { get; set; }
        public virtual Subscriptions Subscription { get; set; }
        [JsonIgnore]
        public virtual ICollection<Payments> Payments { get; set; }
    }
}
