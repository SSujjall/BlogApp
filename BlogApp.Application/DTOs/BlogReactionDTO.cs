using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogApp.Domain.Shared;

namespace BlogApp.Application.DTOs
{
    public class BlogReactionDTO
    {
        public int Id { get; set; }
        public int BlogId { get; set; }
        public string UserId { get; set; }
        public VoteType ReactionType { get; set; }
        public DateOnly CreatedAt { get; set; }
    }

    public class AddOrUpdateBlogReactionDTO
    {
        public int BlogId { get; set; }
        public VoteType ReactionType { get; set; }
    }
}
