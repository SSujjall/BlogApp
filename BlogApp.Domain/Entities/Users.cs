using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BlogApp.Domain.Entities
{
    public class Users : IdentityUser
    {
        // Extra properties for identity user
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }

        [ForeignKey(nameof(CurrentSubscription))]
        public int? CurrentSubscriptionId { get; set; }


        // Virtual navigation properties for relationships
        public virtual ICollection<Blogs> Blogs { get; set; } = new List<Blogs>();
        public virtual ICollection<Comments> Comments { get; set; } = new List<Comments>();
        public virtual ICollection<BlogReaction> BlogReactions { get; set; } = new List<BlogReaction>();
        public virtual ICollection<CommentReaction> CommentReactions { get; set; } = new List<CommentReaction>();
        public virtual ICollection<Notifications> Notifications { get; set; } = new List<Notifications>();
        public virtual Subscriptions CurrentSubscription { get; set; }
        [JsonIgnore]
        public virtual ICollection<Orders> Orders { get; set; } = new List<Orders>();
        public virtual ICollection<Payments> Payments { get; set; } = new List<Payments>();
    }
}
