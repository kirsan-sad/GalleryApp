using GalleryApp.Domain.Interfaces;
using GalleryApp.Domain.Models;
using GalleryApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GalleryApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPhotoRepository _repository;

        public HomeController(IPhotoRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IActionResult> Index()
        {
            var photos = await _repository.GetLastPhotosAsync(4);

            return photos.Count == 0 
                ? NotFound() 
                : (IActionResult)View(photos);
        }

        public async Task<IActionResult> Search(string photoTitle)
        {
            if (photoTitle != null)
            {
                ICollection<Photo> photoList = await _repository.GetPhotosAsync();
                var photoData = photoList.Where(p => p.Title.Contains(photoTitle))
                                          .Select(p => p).ToList();

                return View(photoData);
            }
            else
            {
                return BadRequest();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
