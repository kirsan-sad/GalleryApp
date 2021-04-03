using GalleryApp.Domain.Models;
using GalleryApp.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalleryApp.Web.Models
{
    public class IndexViewModel
    {
        public int GenreId { get; set; }
        public string Genre { get; set; }
        public List<PhotoEntity> Photos { get; set; }
    }
}
