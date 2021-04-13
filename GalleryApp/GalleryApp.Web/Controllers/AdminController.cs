using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GalleryApp.Domain.Interfaces;
using GalleryApp.Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static GalleryApp.Web.Helper;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Microsoft.AspNetCore.Authorization;

namespace GalleryApp.Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IPhotoRepository _photoRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IUserRepository _userRepository;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IPhotoService _photoService;

        public AdminController(IPhotoRepository photoRepository,
                               IGenreRepository genreRepository,
                               IUserRepository userRepository,
                               IWebHostEnvironment appEnvironment,
                               IPhotoService photoService)
        {
            _photoRepository = photoRepository ?? throw new ArgumentNullException(nameof(photoRepository));
            _genreRepository = genreRepository ?? throw new ArgumentNullException(nameof(genreRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _appEnvironment = appEnvironment ?? throw new ArgumentNullException(nameof(appEnvironment));
            _photoService = photoService ?? throw new ArgumentNullException(nameof(photoService));
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.GenresCount = await _genreRepository.GetCount();
            ViewBag.PhotosCount = await _photoRepository.GetCount();
            ViewBag.UsersCount = await _userRepository.GetCount();
            return View();
        }

        public async Task<IActionResult> Genres()
        {
            return View(await _genreRepository.GetGenresAsync());
        }

        // GET: Genres/AddOrEditGenre(Insert)
        // GET: Genres/AddOrEditGenre/5(Update)
        public async Task<IActionResult> AddOrEditGenre(int id = 0)
        {
            if (id == 0)
                return View(new Genre());
            else
            {
                var genre = await _genreRepository.GetByIdAsync(id);

                return genre == null ? NotFound() : (IActionResult)View(genre);
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

        // POST: Genres/DeleteGenre/5
        [HttpPost, ActionName("DeleteGenre")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenreDeleteConfirmed(int id)
        {
            var isDeleted = await _genreRepository.TryDeleteAsync(id);

            if (!isDeleted)
                return NotFound();

            return Json(new { html = Helper.RenderRazorViewToString(this, "_ViewAllGenres", await _genreRepository.GetGenresAsync()) });
        }

        public async Task<IActionResult> Photos()
        {
            return View(await _photoRepository.GetPhotosAsync());
        }

        // GET: Photos/EditPhoto/5(Update)
        public async Task<IActionResult> EditPhoto(int id)
        {
            ViewBag.Genres = await _genreRepository.GetGenresAsync();

            var photo = await _photoRepository.GetByIdAsync(id);

            return photo == null
            ? NotFound()
            : (IActionResult)View(photo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [NoDirectAccess]
        public async Task<IActionResult> EditPhoto([Bind("Index,Name,Title,Genres")] Photo model, List<int> genresId)
        {
            if (ModelState.IsValid)
            {
                var isUpdated = await _photoRepository.TryUpdateAsync(model, genresId);

                if (!isUpdated)
                    return NotFound();

                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllPhotos", await _photoRepository.GetPhotosAsync()) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "EditGenre", model) });
        }

        // GET: Photos/AddPhoto/
        public async Task<IActionResult> AddPhoto()
        {
            ViewBag.Genres = await _genreRepository.GetGenresAsync();

            return View(new Photo());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [NoDirectAccess]
        public async Task<IActionResult> AddPhoto([Bind("Index,Name,Title,Genres")] Photo model, IFormFile uploadedFile, List<int> genresId)
        {
            if (ModelState.IsValid)
            {
                var modelForUploading = await _photoService.UploadingImageOnServer(_appEnvironment.WebRootPath, model, uploadedFile);

                var isUploaded = await _photoRepository.TryUploadAsync(modelForUploading, genresId);

                if (!isUploaded)
                    return NotFound();

                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllPhotos", await _photoRepository.GetPhotosAsync()) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddPhoto", model) });
        }

        // POST: Photos/DeletePhoto/5
        [HttpPost, ActionName("DeletePhoto")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PhotoDeleteConfirmed(int id)
        {
            var photo = await _photoRepository.GetByIdAsync(id);
            var photoName = photo.Name;

            var isEntityDeleted = await _photoRepository.TryDeleteAsync(id);

            if (!isEntityDeleted)
                return NotFound();
            else
            {
                var isImagesDeleted = _photoService.TryDeleteImageFromServer(_appEnvironment.WebRootPath, photoName);

                if (!isImagesDeleted)
                    return NotFound("Files not found");
            }

            return Json(new { html = Helper.RenderRazorViewToString(this, "_ViewAllPhotos", await _photoRepository.GetPhotosAsync()) });
        }

        public async Task<IActionResult> Users()
        {
            return View(await _userRepository.GetUsersAsync());
        }

        // GET: Users/AddOrEditUser(Insert)
        // GET: Users/AddOrEditUser/5(Update)
        public async Task<IActionResult> AddOrEditUser(int id = 0)
        {
            if (id == 0)
                return View(new User());
            else
            {
                var user = await _userRepository.GetByIdAsync(id);

                return user == null
                    ? NotFound()
                    : (IActionResult)View(user);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEditUser(int id, [Bind("Index,Login,Password")] User model)
        {
            if (ModelState.IsValid)
            {
                //Insert
                if (id == 0)
                {
                    var isCreated = await _userRepository.TryCreateAsync(model);

                    if (!isCreated)
                        return NotFound();
                }
                //Update
                else
                {
                    var isUpdated = await _userRepository.TryUpdateAsync(model);

                    if (!isUpdated)
                        return NotFound();
                }
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAllUsers", await _userRepository.GetUsersAsync()) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEditUser", model) });
        }

        // POST: Users/DeleteUser/5
        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserDeleteConfirmed(int id)
        {
            var isDeleted = await _userRepository.TryDeleteAsync(id);

            if (!isDeleted)
                return NotFound();

            return Json(new { html = Helper.RenderRazorViewToString(this, "_ViewAllUsers", await _userRepository.GetUsersAsync()) });
        }
    }
}