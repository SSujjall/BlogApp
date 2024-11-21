using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Domain.Entities
{
    public class Comments
    {
        [Key]
        public int CommendId { get; set; }

        [ForeignKey(nameof(Blogs))]
        public int BlogId { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public string CommentDescription { get; set; }
        public bool IsDeleted { get; set; }
        public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public DateOnly UpdatedAt { get; set; }

        // Navigation Properties
        public virtual Blogs Blogs { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<CommentReaction> Reactions { get; set; } = new List<CommentReaction>();
        public virtual ICollection<CommentHistory> History { get; set; } = new List<CommentHistory>();
    }
}
