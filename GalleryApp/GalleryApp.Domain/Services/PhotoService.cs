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
            string uniqueFileName = Guid.NewGuid().ToString() + ".jpg";

            var fullImagePath = GetFullImagePath(WebRootPath, uniqueFileName);
            var thumbnailsPath = GetthumbnailsPath(WebRootPath, uniqueFileName);

            using (var fileStream = new FileStream(fullImagePath, FileMode.Create))
            using (var image = Image.Load(uploadedFile.OpenReadStream()))
            {
                await uploadedFile.CopyToAsync(fileStream);

                int line = image.Height < image.Width ? image.Height : image.Width;

                var clone = image.Clone(x =>
                x.Crop(line, line)
                .Resize(new ResizeOptions()
                {
                    Mode = ResizeMode.Max,
                    Size = new Size() { Width = 260 }
                }));

                await clone.SaveAsync(thumbnailsPath);
            }

            modelForUploading.Name = uniqueFileName;

            return modelForUploading;
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
                file.Delete();
                filethumb.Delete();
                success = true;
            }
            else
                throw new ArgumentNullException(nameof(file));

            return success;
        }
    }
}
