using BlogApp.Domain.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Domain.Entities
{
    public class BlogReaction
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(Blogs))]
        public int BlogId { get; set; }

        public VoteType ReactionType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation Property
        public virtual Blogs Blogs { get; set; } = null!;
        public virtual Users User { get; set; } = null!;
    }
}
