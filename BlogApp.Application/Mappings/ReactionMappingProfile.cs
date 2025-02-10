using AutoMapper;
using BlogApp.Application.DTOs;
using BlogApp.Domain.Entities;

namespace BlogApp.Application.Mappings
{
    public class ReactionMappingProfile : Profile
    {
        public ReactionMappingProfile()
        {
            #region Blog Reaction Map
            CreateMap<BlogReaction, BlogReactionDTO>();
            CreateMap<BlogReactionDTO, BlogReaction>();

            CreateMap<BlogReaction, AddOrUpdateBlogReactionDTO>();
            CreateMap<AddOrUpdateBlogReactionDTO, BlogReaction>();
            #endregion

            #region Comment Reaction Map
            CreateMap<CommentReaction, CommentReactionDTO>();
            CreateMap<CommentReactionDTO, CommentReaction>();

            CreateMap<CommentReaction, AddOrUpdateCommentReactionDTO>();
            CreateMap<AddOrUpdateCommentReactionDTO, CommentReaction>();
            #endregion
        }
    }
}
