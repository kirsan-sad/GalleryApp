using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GalleryApp.Web.Models;
using GalleryApp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using GalleryApp.Domain.Interfaces;

namespace GalleryApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPhotoRepository _repository;

        public HomeController(ILogger<HomeController> logger, IPhotoRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            IActionResult result;

            var photos = await _repository.GetLastPhotosAsync(5);

            result = (photos.Count == 0) ? NotFound() : result = View(photos);

            return result;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
