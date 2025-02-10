using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogApp.Domain.Entities;

namespace BlogApp.Application.Interface.IRepositories
{
    public interface ICommentReactionRepository : IBaseRepository<CommentReaction>
    {
    }
}
