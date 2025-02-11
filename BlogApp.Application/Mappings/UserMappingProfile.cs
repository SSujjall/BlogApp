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
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            // response map Users response to UserDTO map
            CreateMap<Users, UserDTO>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id)); // mapping Id from Identity table to UserId of UserDTO

            // reverse mapping for request (UserDTO to Users)
            CreateMap<UserDTO, Users>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId)); 
        }
    }
}
