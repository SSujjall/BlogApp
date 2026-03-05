using AutoMapper;
using BlogApp.Application.DTOs;
using BlogApp.Domain.Entities;

namespace BlogApp.Application.Mappings
{
    public class PaymentMappingProfile : Profile
    {
        public PaymentMappingProfile()
        {
            CreateMap<Payments, CreatePaymentDTO>().ReverseMap();
        }
    }
}
