using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GalleryApp.Domain.Interfaces;
using GalleryApp.Domain.Models;
using GalleryApp.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GalleryApp.Web.Controllers
{
    public class PhotoController : Controller
    {
        private readonly IPhotoRepository _repository;
        private readonly IGenreRepository _genreRepository;
        private readonly IWebHostEnvironment _appEnvironment;
        public PhotoController(IPhotoRepository repository, IWebHostEnvironment appEnvironment, IGenreRepository genreRepository)
        {
            _repository = repository;
            _appEnvironment = appEnvironment;
            _genreRepository = genreRepository;
        }

        // GET: /<controller>/

        public async Task<IActionResult> Index()
        {
            return View(await _repository.GetAllPhoto());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int? id)
        {
            id = id ?? throw new ArgumentNullException(nameof(id));

            var result = await _repository.GetAllPhotoByGenreAsync(id);

            if (result.Count == 0)
            {
                return NotFound();
            }

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Upload()
        {
            ViewBag.Genres = await _genreRepository.AllGenresAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(Photo photoToUpload, IFormFile uploadedFile, List<int> GenresId)
        {
            
            if (uploadedFile != null)
            {
                // путь к папке Files
                string uploadsFolder = Path.Combine(_appEnvironment.WebRootPath, "images");
                string uniqueFileName = Guid.NewGuid().ToString() + ".jpg";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }

                //Photo photo = new Photo { Name = uniqueFileName};
                photoToUpload.Name = uniqueFileName;
                photoToUpload.Genres = await GetGenresCollectin(GenresId);
                //photoToUpload.GenreChecked = new List<Genre>();

                var result = await _repository.TryUploadAsync(photoToUpload);

                if (!result)
                {
                    return NotFound();
                }
                else
                    return RedirectToAction("Index");
            }

            return NotFound();
        }

        private async Task<ICollection<Genre>> GetGenresCollectin(List<int> GenresId)
        {
            var checkedGenres = new List<Genre>();

            var allGenreEntity = await _genreRepository.AllGenresAsync();

            foreach (var item in GenresId)
            {
                var result = allGenreEntity.FirstOrDefault(genreEntity => genreEntity.Index == item);
                if (result != null)
                    checkedGenres.Add(result);
            }

            return checkedGenres;
        }
    }
}
