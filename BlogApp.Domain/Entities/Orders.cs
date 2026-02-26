using BlogApp.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public double Amount { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public virtual Users User { get; set; }
        public virtual Subscriptions Subscription { get; set; }
        public virtual ICollection<Payments> Payments { get; set; }
    }
}
