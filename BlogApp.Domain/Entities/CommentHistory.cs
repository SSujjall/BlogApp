using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Domain.Entities
{
    public class CommentHistory
    {
        [Key]
        public int CommentHistoryId { get; set; }

        [ForeignKey(nameof(Comments))]
        public int CommentId { get; set; }

        [ForeignKey(nameof(Users))]
        public string ModifiedByUserId { get; set; }

        public string CommentDescription { get; set; }
        public DateOnly UpdatedAt { get; set; }

        // Virtual navigation property
        public virtual Comments Comments { get; set; } = null!;
        public virtual Users ModifiedBy { get; set; } = null!;
    }
}
