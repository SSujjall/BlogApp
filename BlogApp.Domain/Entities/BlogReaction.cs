using BlogApp.Domain.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Domain.Entities
{
    public class BlogReaction
    {
        [Key]
        public int BlogReactionId { get; set; }

        [ForeignKey(nameof(Blogs))]
        public int BlogId { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public VoteType ReactionType { get; set; }
        public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        // Navigation Properties
        public virtual Blogs Blogs { get; set; }
        public virtual Users User { get; set; }
    }
}
