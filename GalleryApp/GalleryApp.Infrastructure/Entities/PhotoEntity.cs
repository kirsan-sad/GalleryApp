using System;
using System.Collections.Generic;
using System.Text;

namespace GalleryApp.Infrastructure.Entities
{
    public class PhotoEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Format { get; set; }
        public string Path { get; set; }
        public IEnumerable<GenreEntity> Genres { get; set; }
    }
}
