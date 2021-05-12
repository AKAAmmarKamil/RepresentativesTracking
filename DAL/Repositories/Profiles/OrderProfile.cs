using AutoMapper;
using Dto;
using Modle.Model;
using System;

namespace Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            //Source -> Target
            CreateMap<Order, OrderReadDto>();

            CreateMap<OrderWriteDto, Order>().ForMember(x => x.AddOrderDate, opt => opt.MapFrom(x => DateTime.Now));
            CreateMap<Order, OrderWriteDto>();

            CreateMap<OrderStartDto, Order>();
            CreateMap<Order, OrderStartDto>();

            CreateMap<OrderEndDto, Order>();
            CreateMap<Order, OrderEndDto>();
        }
    }
}