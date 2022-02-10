using GalleryApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GalleryApp.Web.Models
{
    public class PhotoModelAPI
    {
        public int Index { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The title must be between 3 and 50 characters long")]
        public string Title { get; set; }
        //public User Author { get; set; }
        public string Name { get; set; }

        [Required]
        public int[] Genres { get; set; }
    }
}
