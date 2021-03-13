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
        public string Format { get; set; }
        public string Path { get; set; }
    }
}
