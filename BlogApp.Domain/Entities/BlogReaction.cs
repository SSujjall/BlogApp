using BlogApp.Domain.Entities.Abstracts;
using BlogApp.Domain.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Domain.Entities
{
    public class BlogReaction : Reaction
    {
        [ForeignKey(nameof(Blogs))]
        public int BlogId { get; set; }

        // Navigation Property
        public virtual Blogs Blogs { get; set; } = null!;
    }
}
