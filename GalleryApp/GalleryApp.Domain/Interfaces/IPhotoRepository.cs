﻿using GalleryApp.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GalleryApp.Domain.Interfaces
{
    public interface IPhotoRepository : IRepository<Photo>
    {
        Task<ICollection<Photo>> GetPhotosByGenreAsync(int genreId);
        Task<bool> TryUploadAsync(Photo photoForUploading, List<int> genresId);
        Task<bool> TryUpdateAsync(Photo photoForUpddating, List<int> genresId);
        Task<ICollection<Photo>> GetPhotosAsync();
        Task<ICollection<LastPhotos>> GetLastPhotosAsync(int numberOfPhotos);
    }
}
