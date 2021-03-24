using System;
using System.Collections.Generic;

namespace GalleryApp.Infrastructure.Entities
{
    public class GenreEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<PhotoEntity> Photos { get; set; }
    }
}
