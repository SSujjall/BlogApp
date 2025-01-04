using Microsoft.AspNetCore.Http;

namespace BlogApp.Application.DTOs
{
    public class BlogsDTO
    {
        public int BlogId { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int UpVoteCount { get; set; }
        public int DownVoteCount { get; set; }
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
        public IFormFile ImageUrl { get; set; }
    }
}
