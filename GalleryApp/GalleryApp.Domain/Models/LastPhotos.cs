using System;
using System.Collections.Generic;
using System.Text;

namespace GalleryApp.Domain.Models
{
    public class LastPhotos
    {
        public int GenreId { get; set; }
        public string Genre { get; set; }
        public ICollection<Photo> Photos { get; set; }
    }
}
