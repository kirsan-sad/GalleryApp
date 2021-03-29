using System;
using System.Collections.Generic;
using System.Text;

namespace GalleryApp.Infrastructure.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public IEnumerable<PhotoEntity> Photos { get; set; }
    }
}
