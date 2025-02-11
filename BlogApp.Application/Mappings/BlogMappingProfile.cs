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
    public class BlogMappingProfile : Profile
    {
        public BlogMappingProfile()
        {
            CreateMap<AddToBlogHistoryDTO, UpdateBlogDTO>();
            CreateMap<BlogHistory, AddToBlogHistoryDTO>();
            CreateMap<AddToBlogHistoryDTO, BlogHistory>();
        }
    }
}
