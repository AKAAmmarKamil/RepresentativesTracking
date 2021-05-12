using AutoMapper;
using Dto;
using Modle.Model;
using System;

namespace Profiles
{
    public class LocationProfile : Profile
    {
        public LocationProfile()
        {
            //Source -> Target
            CreateMap<RepresentativeLocation, LocationReadDto>();

            CreateMap<LocationWriteDto, RepresentativeLocation>().ForMember(x => x.LocationDate, opt => opt.MapFrom(x => DateTime.Now))
                .ForMember(x=>x.IsOnline,opt=>opt.MapFrom(x=>true));
            CreateMap<RepresentativeLocation, LocationWriteDto>();

            CreateMap<LocationOfflineWriteDto, RepresentativeLocation>().ForMember(x => x.IsOnline, opt => opt.MapFrom(x => false));
            CreateMap<RepresentativeLocation, LocationOfflineWriteDto>();
        }
    }
}