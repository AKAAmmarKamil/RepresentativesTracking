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
               .ForMember(x=>x.TotalPriceInIQD,opt=>opt.MapFrom(x=>x.PriceInIQD * x.Count)).
                ForMember(x => x.TotalPriceInUSD, opt => opt.MapFrom(x => x.PriceInUSD * x.Count));
            CreateMap<Products, ProductsByOrderReadDto>()
              .ForMember(x => x.TotalPriceInIQD, opt => opt.MapFrom(x => x.PriceInIQD * x.Count)).
               ForMember(x => x.TotalPriceInUSD, opt => opt.MapFrom(x => x.PriceInUSD * x.Count));
            CreateMap<ProductsWriteDto, Products>();
            CreateMap<Products, ProductsWriteDto>();
          
        }
    }
}