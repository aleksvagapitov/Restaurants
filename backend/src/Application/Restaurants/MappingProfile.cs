

using System.Collections.Generic;
using AutoMapper;
using Domain.Entities;

namespace Application.Restaurants
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RestaurantHours, RestaurantHoursDto>()
                .ForMember(d => d.StartTime, o => o.MapFrom(s => s.StartTime.ToString(@"hh\:mm")))
                .ForMember(d => d.EndTime, o => o.MapFrom(s => s.EndTime.ToString(@"hh\:mm")));
            CreateMap<Restaurant, RestaurantDto>()
                .ForMember(d => d.WorkHours, o => o.MapFrom(s => s.WorkHours))
                .ForMember(d => d.Photos, o => o.MapFrom(s => s.Photos))
                .ForMember(d => d.Latitude, o => o.MapFrom(s => s.Location.Y))
                .ForMember(d => d.Longitude, o => o.MapFrom(s => s.Location.X));
        }
    }
}