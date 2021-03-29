using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GalleryApp.Web.Models
{
    public class RegisterModel
    {
        [Required]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Login must be between 5 and 200 characters long")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
