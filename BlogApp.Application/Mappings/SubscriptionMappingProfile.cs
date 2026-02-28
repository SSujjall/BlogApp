using AutoMapper;
using BlogApp.Application.DTOs;
using BlogApp.Domain.Entities;

namespace BlogApp.Application.Mappings
{
    public class SubscriptionMappingProfile : Profile
    {
        public SubscriptionMappingProfile()
        {
            CreateMap<Subscriptions, CreateSubscriptionDTO>().ReverseMap();
            CreateMap<Subscriptions, UpdateSubscriptionDTO>().ReverseMap();
        }
    }
}
