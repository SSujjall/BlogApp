using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BlogApp.Domain.Shared;
using BlogApp.Domain.Entities.Abstracts;

namespace BlogApp.Domain.Entities
{
    public class CommentReaction : Reaction
    {
        [ForeignKey(nameof(Comments))]
        public int CommentId { get; set; }

        // Navigation Property
        public virtual Comments Comments { get; set; } = null!;
    }
}
