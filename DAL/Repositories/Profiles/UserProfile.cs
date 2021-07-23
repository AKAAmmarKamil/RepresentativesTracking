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
            CreateMap<User, UserReadDto>().ForMember(x=>x.CompanyName,opt=>opt.MapFrom(x=>x.Company.Name))
                .ForMember(x=>x.Type,opt=>opt.MapFrom(x=>Converter(x.Type)));
            CreateMap<UserWriteDto, User>();
            CreateMap<User, UserWriteDto>();
        }
        private static string Converter(int Type)
        {
            if (Type == 1) return "تسليم";
            else if (Type == 2) return "إستلام";
            else return "ترويج";
        }
    }
}
