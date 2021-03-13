using GalleryApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GalleryApp.Domain.Interfaces
{
    public interface IPhotoRepository : IRepository<Photo>
    {
        ICollection<Photo> GetAllPhotoByGenre(string genre);
        bool TryUpload(Photo photoForLoading);
    }
}
