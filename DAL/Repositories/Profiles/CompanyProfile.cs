using AutoMapper;
using Dto;
using Modle.Model;
namespace Profiles
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            //Source -> Target
            CreateMap<Company, CompanyReadDto>();
            CreateMap<CompanyWriteDto, Company>().ForMember(x=>x.RepresentativeCount,opt=>opt.MapFrom(x=>15));
            CreateMap<Company, CompanyWriteDto>();
            CreateMap<CompanyUpdateDto, Company>();
            CreateMap<Company, CompanyUpdateDto>();
        }
    }
}