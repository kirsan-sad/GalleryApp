using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GalleryApp.Domain.Models
{
    public class Genre
    {
        public int Index { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The genre name must be between 3 and 50 characters long")]
        public string Name { get; set; }

        [StringLength(200, MinimumLength = 5, ErrorMessage = "The genre description must be between 5 and 200 characters long")]
        public string Description { get; set; }
        public IEnumerable<Photo> Photos { get; set; }
    }
}
