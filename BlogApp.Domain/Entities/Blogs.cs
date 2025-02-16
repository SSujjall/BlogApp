using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Domain.Entities
{
    public class Blogs
    {
        [Key]
        public int BlogId { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public int PopularityScore { get; set; } = 0;
        public int UpVoteCount { get; set; } = 0;
        public int DownVoteCount { get; set; } = 0;
        public int CommentCount { get; set; } = 0;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; }

        // Navigation Properties
        public virtual Users User { get; set; } = null!;
        public virtual ICollection<Comments> Comments { get; set; } = new List<Comments>();
        public virtual ICollection<BlogReaction> Reactions { get; set; } = new List<BlogReaction>();
        public virtual ICollection<BlogHistory> History { get; set; } = new List<BlogHistory>();
    }
}