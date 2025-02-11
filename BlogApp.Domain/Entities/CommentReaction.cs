using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BlogApp.Domain.Shared;

namespace BlogApp.Domain.Entities
{
    public class CommentReaction
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(Comments))]
        public int CommentId { get; set; }

        public VoteType ReactionType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation Property
        public virtual Comments Comments { get; set; } = null!;
        public virtual Users User { get; set; } = null!;
    }
}
