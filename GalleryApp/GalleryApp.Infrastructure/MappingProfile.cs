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
            CreateMap<PhotoEntity, Photo>();
            CreateMap<GenreEntity, Genre>();
        }
    }
}
