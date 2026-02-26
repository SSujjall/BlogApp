using System.ComponentModel.DataAnnotations;

namespace BlogApp.Domain.Entities
{
    public class Subscriptions
    {
        [Key]
        public int SubscriptionId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int DurationInMonths { get; set; }
        public string? Description { get; set; }

        // Navigation Property
        public virtual ICollection<Orders> Orders { get; set; } = new List<Orders>();
        public virtual ICollection<Users> Users { get; set; } = new List<Users>();
    }
}
