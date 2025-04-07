using AutoMapper;
using Core.DTOs;
using Core.Models;

namespace WeatherMVCApp.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<WeatherRecord, WeatherDTO>().ReverseMap();
        }
    }
}
