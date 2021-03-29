using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GalleryApp.Domain.Interfaces;
using GalleryApp.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GalleryApp.Web.Controllers
{
    public class GenreController : Controller
    {
        private readonly IGenreRepository _repository;
        public GenreController(IGenreRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IActionResult> Index()
        {
            return View(await _repository.GetGenresAsync());
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
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
        public async Task<ActionResult<Genre>>Update([Required]int id)
        {
            var genre = await _repository.GetByIdAsync(id);

            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        [HttpPost]
        public async Task<IActionResult>Update(Genre model)
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
