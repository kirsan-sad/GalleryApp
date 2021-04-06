using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalleryApp.Domain.Interfaces;
using GalleryApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using static GalleryApp.Web.Helper;

namespace GalleryApp.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IPhotoRepository _photoRepository;
        private readonly IGenreRepository _genreRepository;

        public AdminController(IPhotoRepository photoRepository, IGenreRepository genreRepository)
        {
            _photoRepository = photoRepository ?? throw new ArgumentNullException(nameof(photoRepository));
            _genreRepository = genreRepository ?? throw new ArgumentNullException(nameof(genreRepository));
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Genres()
        {
            return View(await _genreRepository.GetGenresAsync());
        }

        // GET: Genres/AddOrEdit(Insert)
        // GET: Genres/AddOrEdit/5(Update)
        public async Task<IActionResult> AddOrEditGenre(int id = 0)
        {
            if (id == 0)
                return View(new Genre());
            else
            {
                var genre = await _genreRepository.GetByIdAsync(id);
                if (genre == null)
                {
                    return NotFound();
                }
                return View(genre);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEditGenre(int id, [Bind("Index,Name,Description")] Genre model)
        {
            if (ModelState.IsValid)
            {
                //Insert
                if (id == 0)
                {
                    var isCreated = await _genreRepository.TryCreateAsync(model);

                    if (!isCreated)
                        return NotFound();
                }
                //Update
                else
                {
                    var isUpdated = await _genreRepository.TryUpdateAsync(model);

                    if (!isUpdated)
                        return NotFound();
                }
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllGenres", await _genreRepository.GetGenresAsync()) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEditGenre", model) });
        }

        // POST: Genres/Delete/5
        [HttpPost, ActionName("DeleteGenre")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenreDeleteConfirmed(int id)
        {
            var isDeleted = await _genreRepository.TryDeleteAsync(id);

            if (!isDeleted)
                return NotFound();

            return Json(new { html = Helper.RenderRazorViewToString(this, "_ViewAllGenres", await _genreRepository.GetGenresAsync()) });
        }

    }
}