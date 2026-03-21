using AutoMapper;
using BlogApp.Application.DTOs.PaymentDTOs;
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
