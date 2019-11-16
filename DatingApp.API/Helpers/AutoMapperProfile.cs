using AutoMapper;
using DatingApp.API.Models;
using DatingApp.API.Dtos;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace DatingApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
           CreateMap<User, UserForListDto>()
            .ForMember(dest => dest.PhotoUrl, opt  => {
                opt.MapFrom(src => src.Photos.FirstOrDefault(x=>x.IsMain).Url);
            })
            .ForMember(dest =>dest.Age, opt => opt.MapFrom(d => d.DateOfBirth.CalculateAge()));

            CreateMap<User, UserForDetailedDto>()
                .ForMember(dest => dest.PhotoUrl, opt => {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                })
                .ForMember(dest => dest.Age, opt => {
                    opt.MapFrom(d => d.DateOfBirth.CalculateAge());
                });  
            CreateMap<Photo, PhotosForDetailedDto>();
        }
    }
}