using System;
using System.Collections.Generic;
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
            return View(await _repository.AllGenresAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Genre genreForCreate)
        {
            if (ModelState.IsValid)
            {
                var result = await _repository.TryCreateAsync(genreForCreate);

                if (!result)
                {
                    return NotFound();
                }
                else
                    return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<ActionResult<Genre>> Get(int? id)
        {
            id = id ?? throw new ArgumentNullException(nameof(id));

            var result = await _repository.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }

        [HttpGet]
        public async Task<ActionResult<Genre>> Edit(int? id)
        {
            id = id ?? throw new ArgumentNullException(nameof(id));

            var result = await _repository.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Genre genreForUpdate)
        {
            if (ModelState.IsValid)
            {
                var result = await _repository.TryUpdateAsync(genreForUpdate);

                if (!result)
                {
                    return NotFound();
                }
                else
                    return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<ActionResult<Genre>> Delete(int? id)
        {
            id = id ?? throw new ArgumentNullException(nameof(id));

            var result = await _repository.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Genre genreForUpdate)
        {
            var result = await _repository.TryDeleteAsync(genreForUpdate);

            if (!result)
            {
                return NotFound();
            }
            else
                return RedirectToAction(nameof(Index));
        }
    }
}
