﻿using BlogApp.Domain.Shared;

namespace BlogApp.Application.DTOs
{
    public class BlogReactionDTO
    {
        public int Id { get; set; }
        public int BlogId { get; set; }
        public string UserId { get; set; }
        public VoteType ReactionType { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class GetAllUserReactionDTO
    {
        public int BlogId { get; set; }
        public VoteType ReactionType { get; set; }
    }

    public class AddOrUpdateBlogReactionDTO
    {
        public int BlogId { get; set; }
        public VoteType ReactionType { get; set; }
    }
}
