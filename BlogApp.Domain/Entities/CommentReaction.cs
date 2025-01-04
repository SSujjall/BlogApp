using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BlogApp.Domain.Shared;

namespace BlogApp.Domain.Entities
{
    public class CommentReaction
    {
        [Key]
        public int CommentReactionId { get; set; }

        [ForeignKey(nameof(Comments))]
        public int CommentId { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public VoteType ReactionType { get; set; }
        public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        // Navigation Properties
        public virtual Comments Comments { get; set; } = null!;
        public virtual Users User { get; set; } = null!;
    }
}
