using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GalleryApp.Domain.Interfaces;
using GalleryApp.Domain.Models;
using GalleryApp.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;


namespace GalleryApp.Web.Controllers
{
    public class PhotoController : Controller
    {
        private readonly IPhotoRepository _repository;
        private readonly IGenreRepository _genreRepository;
        private readonly IWebHostEnvironment _appEnvironment;
        public PhotoController(IPhotoRepository repository, IWebHostEnvironment appEnvironment, IGenreRepository genreRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _appEnvironment = appEnvironment ?? throw new ArgumentNullException(nameof(appEnvironment));
            _genreRepository = genreRepository ?? throw new ArgumentNullException(nameof(genreRepository));
        }

        public async Task<IActionResult> Index()
        {
            return View(await _repository.GetPhotos());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Get([Required]int id)
        {
            IActionResult result;

            var photos = await _repository.GetPhotosByGenreAsync(id);

            result = (photos.Count == 0) ? NotFound() : result = View(photos);

            return result;
        }

        [HttpGet]
        public async Task<IActionResult> Upload()
        {
            ViewBag.Genres = await _genreRepository.GetGenresAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(Photo model, IFormFile uploadedFile, List<int> genresId)
        {
            if (uploadedFile != null)
            {
                string uploadsFolder = Path.Combine(_appEnvironment.WebRootPath, "images");
                string uploadsThumbnailsFolder = Path.Combine(_appEnvironment.WebRootPath, "images/thumbnails");
                string uniqueFileName = Guid.NewGuid().ToString() + ".jpg";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                string thumbnailsFilePath = Path.Combine(uploadsThumbnailsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                using (var image = Image.Load(uploadedFile.OpenReadStream()))
                {
                    await uploadedFile.CopyToAsync(fileStream);

                    var clone = image.Clone(x =>
                    x.Resize(
                             new ResizeOptions()
                             {
                                 Mode = ResizeMode.Max,
                                 Size = new Size() { Width = 250 }
                             }
                            ));
                    await clone.SaveAsync(thumbnailsFilePath);
                }

                //using (var image = Image.Load(uploadedFile.OpenReadStream()))
                //{
                //    var clone = image.Clone(x =>
                //    x.Resize(
                //             new ResizeOptions()
                //             {
                //                 Mode = ResizeMode.Max,
                //                 Size = new Size() { Width = 450 }
                //             }
                //            ));
                //    await clone.SaveAsync(thumbnailsFilePath); // Automatic encoder selected based on extension.
                //}


                model.Name = uniqueFileName;

                var isUploaded = await _repository.TryUploadAsync(model, genresId);

                if (!isUploaded)
                {
                    return NotFound();
                }
                else
                    return RedirectToAction("Index");
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<ActionResult<Photo>> Delete([Required]int id)
        {
            ActionResult result;

            var photo = await _repository.GetByIdAsync(id);

            result = (photo == null) ? NotFound() : result = View(photo);

            return result;
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Photo model)
        {
            IActionResult result;

            string uploadsFolder = Path.Combine(_appEnvironment.WebRootPath, "images");
            string filePath = Path.Combine(uploadsFolder, model.Name);

            var isDeleted = await _repository.TryDeleteAsync(model);

            if (!isDeleted)
                result = NotFound();
            else
            {
                FileInfo file = new FileInfo(filePath);

                if (file.Exists)
                    file.Delete();
                else
                    throw new ArgumentNullException(nameof(file));

                result = RedirectToAction(nameof(Index));
            }

            return result;
        }

        [HttpGet]
        public async Task<ActionResult<Photo>> Update([Required]int id)
        {
            ActionResult result;

            var photo = await _repository.GetByIdAsync(id);

            result = (photo == null) ? NotFound() : result = View(photo);

            return result;
        }

        [HttpPost]
        public async Task<IActionResult> Update(Photo model)
        {
            if (ModelState.IsValid)
            {
                var isUpdated = await _repository.TryUpdateAsync(model);

                if (!isUpdated)
                {
                    return NotFound();
                }
                else
                    return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }
    }
}
