using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GalleryApp.Domain.Interfaces;
using GalleryApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace GalleryApp.Web.Controllers
{
    public class GenreController : Controller
    {
        private readonly IGenreRepository _repository;
        public GenreController(IGenreRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _repository.GetGenresAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Genre model)
        {
            if (ModelState.IsValid)
            {
                var isCreated = await _repository.TryCreateAsync(model);

                if (!isCreated)
                {
                    return NotFound();
                }
                else
                    return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<ActionResult<Genre>> Get([Required]int id)
        {
            var genre = await _repository.GetByIdAsync(id);

            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        [HttpGet]
        public async Task<ActionResult<Genre>> Edit([Required]int id)
        {
            var genre = await _repository.GetByIdAsync(id);

            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateGenre(Genre model)
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

        [HttpGet]
        public async Task<ActionResult<Genre>> Delete([Required]int id)
        {
            var genre = await _repository.GetByIdAsync(id);

            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Genre model)
        {
            IActionResult result;

            var isDeleted = await _repository.TryDeleteAsync(model);

            result = (!isDeleted)? NotFound() : result = RedirectToAction(nameof(Index));

            return result;
        }
    }
}
