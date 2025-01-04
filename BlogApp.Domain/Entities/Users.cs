using Microsoft.AspNetCore.Identity;

namespace BlogApp.Domain.Entities
{
    public class Users : IdentityUser
    {
        // Virtual navigation properties for relationships
        public virtual ICollection<Blogs> Blogs { get; set; } = new List<Blogs>();
        public virtual ICollection<Comments> Comments { get; set; } = new List<Comments>();
        public virtual ICollection<BlogReaction> BlogReactions { get; set; } = new List<BlogReaction>();
        public virtual ICollection<CommentReaction> CommentReactions { get; set; } = new List<CommentReaction>();
        public virtual ICollection<Notifications> Notifications { get; set; } = new List<Notifications>();
    }
}
