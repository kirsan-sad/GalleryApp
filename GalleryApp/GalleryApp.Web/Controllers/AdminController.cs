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

namespace GalleryApp.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IPhotoRepository _photoRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IUserRepository _userRepository;
        private readonly IWebHostEnvironment _appEnvironment;

        public AdminController(IPhotoRepository photoRepository,
                               IGenreRepository genreRepository,
                               IUserRepository userRepository,
                               IWebHostEnvironment appEnvironment)
        {
            _photoRepository = photoRepository ?? throw new ArgumentNullException(nameof(photoRepository));
            _genreRepository = genreRepository ?? throw new ArgumentNullException(nameof(genreRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _appEnvironment = appEnvironment ?? throw new ArgumentNullException(nameof(appEnvironment));
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

            if (photo == null)
            {
                return NotFound();
            }
            return View(photo);
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

                model.Name = uniqueFileName;

                var isUploaded = await _photoRepository.TryUploadAsync(model, genresId);

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

            string uploadsFolder = Path.Combine(_appEnvironment.WebRootPath, "images");
            string uploadsThumbnailsFolder = Path.Combine(_appEnvironment.WebRootPath, "images/thumbnails");
            string filePath = Path.Combine(uploadsFolder, photo.Name);
            string thumbnailsFilePath = Path.Combine(uploadsThumbnailsFolder, photo.Name);

            var isDeleted = await _photoRepository.TryDeleteAsync(id);

            if (!isDeleted)
                return NotFound();
            else
            {
                FileInfo file = new FileInfo(filePath);
                FileInfo filethumb = new FileInfo(thumbnailsFilePath);

                if (file.Exists && filethumb.Exists)
                {
                    file.Delete();
                    filethumb.Delete();
                }
                else
                    throw new ArgumentNullException(nameof(file));
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
                if (user == null)
                {
                    return NotFound();
                }
                return View(user);
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