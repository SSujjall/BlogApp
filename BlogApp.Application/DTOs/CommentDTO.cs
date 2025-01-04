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
        public int CommendId { get; set; }
        public int BlogId { get; set; }
        public string UserId { get; set; }
        public string CommentDescription { get; set; }
        public DateOnly CreatedAt { get; set; }
        public DateOnly UpdatedAt { get; set; }
    }

    public class AddCommentDTO
    {
        public int CommendId { get; set; }
        public int BlogId { get; set; }
        public string UserId { get; set; }
        public string CommentDescription { get; set; }
    }
}
