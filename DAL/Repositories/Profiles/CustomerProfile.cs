using AutoMapper;
using Dto;
using Modle.Model;
namespace Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            //Source -> Target
            CreateMap<Customer, CustomerReadDto>();
            CreateMap<Customer, CustomerForOrderReadDto>().ForMember(x=>x.Company,opt=>opt.MapFrom(x=>x.Company.Name));
            CreateMap<CustomerWriteDto, Customer>();
            CreateMap<Customer, CustomerWriteDto>();
        }
    }
}