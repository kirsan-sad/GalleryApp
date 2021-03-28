using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GalleryApp.Domain.Models
{
    public class Photo
    {
        public int Index { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The genre name must be between 3 and 50 characters long")]
        public string Title { get; set; }
        public string Author { get; set; }
        public string Name { get; set; }
        public IEnumerable<Genre> Genres { get; set; }
    }
}
