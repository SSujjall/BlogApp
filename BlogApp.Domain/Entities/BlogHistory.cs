using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Domain.Entities
{
    public class BlogHistory
    {
        [Key]
        public int BlogHistoryId { get; set; }
        
        [ForeignKey(nameof(Blog))]
        public int BlogId { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        // Virtual navigation property
        public virtual Blogs Blog { get; set; } = null!;
        public virtual Users User { get; set; } = null!;
    }
}
