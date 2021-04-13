using GalleryApp.Domain.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GalleryApp.Domain.Interfaces
{
    public interface IPhotoService
    {
        Task<Photo> UploadingImageOnServer(string WebRootPath, Photo modelForUploading, IFormFile uploadedFile);
        bool TryDeleteImageFromServer(string WebRootPath, string photoName);
    }
}
