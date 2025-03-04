using BlogApp.Domain.Shared;

namespace BlogApp.Application.DTOs
{
    public class CommentReactionDTO
    {
        public int Id { get; set; }
        public int CommentId { get; set; }
        public string UserId { get; set; }
        public VoteType ReactionType { get; set; }
        public DateOnly CreatedAt { get; set; }
    }

    public class AddOrUpdateCommentReactionDTO
    {
        public int CommentId { get; set; }
        public VoteType ReactionType { get; set; }
    }

    public class GetAllUserCommentReactionDTO
    {
        public int CommentId { get; set; }
        public VoteType ReactionType { get; set; }
    }
}
