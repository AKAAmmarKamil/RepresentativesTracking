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
            CreateMap<Order, OrderReadDto>().ForMember(x=>x.Status,opt=>opt.MapFrom(x=>Convert(x.Status)));
                //.ForMember(x => x.TotalPriceInIQD, opt => opt.MapFrom(x => x.Products.Count*x.PriceInIQD))
                  //                          .ForMember(x => x.TotalPriceInUSD, opt => opt.MapFrom(x => x.Count * x.PriceInUSD));

            CreateMap<OrderWriteDto, Order>().ForMember(x => x.AddOrderDate, opt => opt.MapFrom(x => DateTime.Now));
            CreateMap<Order, OrderWriteDto>();

            CreateMap<OrderStartDto, Order>();
            CreateMap<Order, OrderStartDto>();

            CreateMap<OrderDeliveryDto, Order>();
            CreateMap<Order, OrderDeliveryDto>();
        }
        public static string Convert(int Status)
        {
            if (Status == 0) return "في الإنتظار";
            if (Status == 1) return "قيد التوصيل";
            if (Status == 2) return "تم التوصيل";
            if (Status == 3) return "مرجع";
            return "";
        }
    }
}