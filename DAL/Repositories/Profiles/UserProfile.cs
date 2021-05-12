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
            CreateMap<User, UserReadDto>();
            CreateMap<UserWriteDto, User>();
            CreateMap<User, UserWriteDto>();
        }
    }
}
