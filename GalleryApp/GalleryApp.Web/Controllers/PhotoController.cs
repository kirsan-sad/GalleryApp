using GalleryApp.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;


namespace GalleryApp.Web.Controllers
{
    public class PhotoController : Controller
    {
        private readonly IPhotoRepository _repository;

        public PhotoController(IPhotoRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        public async Task<IActionResult> Genre([Required]int id)
        {
            var photos = await _repository.GetPhotosByGenreAsync(id);

            return photos.Count == 0
                ? NotFound()
                : (IActionResult)View(photos);
        }
    }
}
