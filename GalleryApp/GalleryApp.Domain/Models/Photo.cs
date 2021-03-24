using System;
using System.Collections.Generic;
using System.Text;

namespace GalleryApp.Domain.Models
{
    public class Photo
    {
        public int Index { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Name { get; set; }
        public IEnumerable<Genre> Genres { get; set; }
    }
}
