using BlogApp.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace BlogApp.Application.DTOs
{
    public class BlogsDTO
    {
        public int BlogId { get; set; }
        public BlogUser User { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int UpVoteCount { get; set; }
        public int DownVoteCount { get; set; }
        public int CommentCount { get; set; }
    }

    public class GetFilteredBlogsDTO
    {
        public IEnumerable<Blogs> Blogs { get; set; }
        public int Count { get; set; }
    }

    public class BlogUser
    {
        public string UserId { get; set; }
        public string Name { get; set; }
    }

    public class CreateBlogDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile ImageUrl { get; set; }
    }

    public class UpdateBlogDTO
    {
        public int BlogId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile? ImageUrl { get; set; }
    }
}
