using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Domain.Entities
{
    public class CommentHistory
    {
        [Key]
        public int CommentHistoryId { get; set; }

        [ForeignKey(nameof(Comment))]
        public int CommentId { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public string CommentDescription { get; set; }
        public DateOnly UpdatedAt { get; set; }

        // Virtual navigation property
        public virtual Comments Comment { get; set; } = null!;
        public virtual Users User { get; set; } = null!;
    }
}
