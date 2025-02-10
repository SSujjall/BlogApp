using AutoMapper;
using BlogApp.Application.DTOs;
using BlogApp.Domain.Entities;

namespace BlogApp.Application.Mappings
{
    public class ReactionMappingProfile : Profile
    {
        public ReactionMappingProfile()
        {
            CreateMap<BlogReaction, BlogReactionDTO>();
            CreateMap<BlogReactionDTO, BlogReaction>();

            CreateMap<BlogReaction, AddBlogReactionDTO>();
            CreateMap<AddBlogReactionDTO, BlogReaction>();
        }
    }
}
