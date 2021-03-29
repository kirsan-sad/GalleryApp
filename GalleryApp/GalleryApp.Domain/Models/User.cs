using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GalleryApp.Domain.Models
{
    public class User
    {
        public int Index { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Login must be between 5 and 200 characters long")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public IEnumerable<Photo> Photos { get; set; }
    }
}
