using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
        [JsonIgnore]
        public virtual ICollection<Orders> Orders { get; set; } = new List<Orders>();
        [JsonIgnore]
        public virtual ICollection<Users> Users { get; set; } = new List<Users>();
    }
}
