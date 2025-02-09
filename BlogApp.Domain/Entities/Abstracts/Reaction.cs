using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogApp.Domain.Shared;

namespace BlogApp.Domain.Entities.Abstracts
{
    public abstract class Reaction
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = null!;

        public VoteType ReactionType { get; set; }
        public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        // Navigation Property
        public virtual Users User { get; set; } = null!;
    }
}
