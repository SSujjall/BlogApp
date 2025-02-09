using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogApp.Domain.Shared;

namespace BlogApp.Application.DTOs
{
    public class ReactionDTO
    {
        public string EntityName { get; set; }
        public int EntityId { get; set; }
        public string UserId { get; set; }
        public VoteType ReactionType { get; set; }
        public DateOnly CreatedAt { get; set; }
    }
}
