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
            CreateMap<CustomerWriteDto, Customer>();
            CreateMap<Customer, CustomerWriteDto>();
        }
    }
}