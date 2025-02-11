using BlogApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.DTOs
{
    public class CommentDTO
    {
        public int CommentId { get; set; }
        public int BlogId { get; set; }
        public string UserId { get; set; }
        public string CommentDescription { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class AddCommentDTO
    {
        public int BlogId { get; set; }
        public string CommentDescription { get; set; }
    }

    public class UpdateCommentDTO
    {
        public int CommentId { get; set; }
        public string CommentDescription { get; set; }
    }
}
