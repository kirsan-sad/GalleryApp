using AutoMapper;
using GalleryApp.Domain.Models;
using GalleryApp.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GalleryApp.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PhotoEntity, Photo>()
                .ForMember(d => d.Index, map => map.MapFrom(s => s.Id))
                .ReverseMap();
            CreateMap<GenreEntity, Genre>()
                .ForMember(d => d.Index, map => map.MapFrom(s => s.Id))
                .ReverseMap();
            CreateMap<UserEntity, User>()
                .ForMember(d => d.Index, map => map.MapFrom(s => s.Id))
                .ReverseMap();
        }
    }
}
