﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GalleryApp.Infrastructure.Entities
{
    public class PhotoEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public UserEntity Author { get; set; }
        public string Name { get; set; }
        public List<GenreEntity> Genres { get; set; }
        public PhotoEntity()
        {
            Genres = new List<GenreEntity>();
        }
    }
}
