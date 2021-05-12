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
            CreateMap<CompanyWriteDto, Company>();
            CreateMap<Company, CompanyWriteDto>();
        }
    }
}