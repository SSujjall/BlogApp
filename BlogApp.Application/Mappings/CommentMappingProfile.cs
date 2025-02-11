using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BlogApp.Application.DTOs;
using BlogApp.Domain.Entities;

namespace BlogApp.Application.Mappings
{
    public class CommentMappingProfile : Profile
    {
        public CommentMappingProfile()
        {
            CreateMap<Comments, CommentHistory>()
                .ForMember(dest => dest.User, opt => opt.Ignore())  // Exclude the User navigation property
                .ForMember(dest => dest.Comment, opt => opt.Ignore()); // Exclude the Comment navigation property
        }
    }
}
