using AutoMapper;
using Dto;
using Model;
using Modle.Model;

namespace Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            //Source -> Target
            CreateMap<User, UserReadDto>().ForMember(x=>x.CompanyName,opt=>opt.MapFrom(x=>x.Company.Name));
            CreateMap<UserWriteDto, User>();
            CreateMap<User, UserWriteDto>();
        }
    }
}
