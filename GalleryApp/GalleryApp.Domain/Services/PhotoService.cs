using GalleryApp.Domain.Interfaces;
using GalleryApp.Domain.Models;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GalleryApp.Domain.Services
{
    public class PhotoService : IPhotoService
    {
        private const string _jpegFileExtension = ".jpeg";
        private const int _resizeWidth = 260;

        private string GetFullImagePath(string WebRootPath, string uniqueFileName)
        {
            string uploadsFolder = Path.Combine(WebRootPath, "images");

            return Path.Combine(uploadsFolder, uniqueFileName);
        }

        private string GetthumbnailsPath(string WebRootPath, string uniqueFileName)
        {
            string uploadThumbnailsFolder = Path.Combine(WebRootPath, "images\\thumbnails");

            return Path.Combine(uploadThumbnailsFolder, uniqueFileName);
        }

        public async Task<Photo> UploadingImageOnServer(string WebRootPath, Photo modelForUploading, IFormFile uploadedFile)
        {
            string uniqueFileName = Guid.NewGuid().ToString() + _jpegFileExtension;

            var fullImagePath = GetFullImagePath(WebRootPath, uniqueFileName);
            var thumbnailsPath = GetthumbnailsPath(WebRootPath, uniqueFileName);

            using (var fileStream = new FileStream(fullImagePath, FileMode.Create))
            using (var image = Image.Load(uploadedFile.OpenReadStream()))
            {
                await uploadedFile.CopyToAsync(fileStream);

                GetThumbnail(image, thumbnailsPath);
            }

            modelForUploading.Name = uniqueFileName;

            return modelForUploading;
        }

        private async void GetThumbnail(SixLabors.ImageSharp.Image image, string thumbnailsPath)
        {
            int line = image.Height < image.Width ? image.Height : image.Width;

            var clone = image.Clone(x =>
            x.Crop(line, line)
            .Resize(new ResizeOptions()
            {
                Mode = ResizeMode.Max,
                Size = new Size() { Width = _resizeWidth }
            }));

            await clone.SaveAsync(thumbnailsPath);
        }

        public bool TryDeleteImageFromServer(string WebRootPath, string photoName)
        {
            bool success = false;

            var fullImagePath = GetFullImagePath(WebRootPath, photoName);
            var thumbnailsPath = GetthumbnailsPath(WebRootPath, photoName);

            FileInfo file = new FileInfo(fullImagePath);
            FileInfo filethumb = new FileInfo(thumbnailsPath);

            if (file.Exists && filethumb.Exists)
            {
                try
                {
                    file.Delete();
                    filethumb.Delete();
                    success = true;
                }
                catch (Exception e)
                {
                    throw;
                }
                
            }
            else
                throw new ArgumentNullException(nameof(file));

            return success;
        }
    }
}
