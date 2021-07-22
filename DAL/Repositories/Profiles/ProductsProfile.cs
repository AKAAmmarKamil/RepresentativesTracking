using AutoMapper;
using Dto;
using Modle.Model;
namespace Profiles
{
    public class ProductsProfile : Profile
    {
        public ProductsProfile()
        {
            //Source -> Target
            CreateMap<Products, ProductsReadDto>()
               .ForMember(x=>x.TotalPriceInIQD,opt=>opt.MapFrom(x=>x.PriceInIQD * x.Quantity)).
                ForMember(x => x.TotalPriceInUSD, opt => opt.MapFrom(x => x.PriceInUSD * x.Quantity));
            CreateMap<Products, ProductsByOrderReadDto>()
              .ForMember(x => x.TotalPriceInIQD, opt => opt.MapFrom(x => x.PriceInIQD * x.Quantity)).
               ForMember(x => x.TotalPriceInUSD, opt => opt.MapFrom(x => x.PriceInUSD * x.Quantity));
            CreateMap<ProductsWriteDto, Products>();
            CreateMap<Products, ProductsWriteDto>();
          
        }
    }
}