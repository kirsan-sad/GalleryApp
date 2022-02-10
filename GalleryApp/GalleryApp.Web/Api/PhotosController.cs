using AutoMapper;
using GalleryApp.Domain.Interfaces;
using GalleryApp.Domain.Models;
using GalleryApp.Infrastructure;
using GalleryApp.Infrastructure.Entities;
using GalleryApp.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalleryApp.Web.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IPhotoRepository _repository;
        private readonly IPhotoService _photoService;
        private readonly GalleryContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _appEnvironment;

        public PhotosController(IPhotoRepository repository, GalleryContext context, IMapper mapper, IPhotoService photoService, IWebHostEnvironment appEnvironment)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _photoService = photoService ?? throw new ArgumentNullException(nameof(photoService));
            _appEnvironment = appEnvironment ?? throw new ArgumentNullException(nameof(appEnvironment));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var photosQuery = from Photo in _context.Photos
                              select new PhotoModelAPI
                              {
                                  Index = Photo.Id,
                                  Title = Photo.Title,
                                  Name = Photo.Name,
                                  //Author = _mapper.Map<User>(Photo.Author),
                                  Genres = Photo.Genres.Select(g => g.Id).ToArray()
                              };

            var photos = await photosQuery.ToListAsync();

            if (photos == null) return NotFound();

            return Ok(photos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var photoQuery = from Photo in _context.Photos
                             where Photo.Id == id
                             select new PhotoModelAPI
                             {
                                 Index = Photo.Id,
                                 Title = Photo.Title,
                                 Name = Photo.Name,
                                 //Author = _mapper.Map<User>(Photo.Author),
                                 Genres = Photo.Genres.Select(g => g.Id).ToArray()
                             };

            var photo = await photoQuery.FirstOrDefaultAsync();

            if (photo == null) return NotFound();

            return Ok(photo);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] PhotoModelAPI apiModel, IFormFile uploadedFile)
        {
            if (uploadedFile == null)
                return UnprocessableEntity("Photo is required");

            var bModel = new Photo {
                Title = apiModel.Title
            };

            var modelForUploading = await _photoService.UploadingImageOnServer(_appEnvironment.WebRootPath, bModel, uploadedFile);

            List<int> genresId = apiModel.Genres.ToList();

            var photoEntityExist = await _context.Photos
               .AnyAsync(photoEntity => photoEntity.Name == modelForUploading.Name);

            if (photoEntityExist == true)
                return NotFound();

            var entityPhoto = _mapper.Map<PhotoEntity>(modelForUploading);
            var photo = _context.Photos.Add(entityPhoto).Entity;
            await _context.SaveChangesAsync();

            photo.Genres = _context.Genres
                .Where(genre => genresId
                .Contains(genre.Id))
                .ToList();

            _context.Update(photo);
            await _context.SaveChangesAsync();

            var photoApi = new PhotoModelAPI
            {
                Index = photo.Id,
                Title = photo.Title,
                Name = photo.Name,
                //Author = _mapper.Map<User>(photo.Author),
                Genres = photo.Genres.Select(g => g.Id).ToArray()
            };

            return CreatedAtAction("GetById", new { id = photoApi.Index }, photoApi);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PhotoModelAPI apiModel)
        {
            var entityForUpdate = await _context.Photos
                .Include(g => g.Genres)
                .FirstOrDefaultAsync(photoEntity => photoEntity.Id == apiModel.Index);

            if (entityForUpdate == null)
                return NotFound();

            entityForUpdate.Title = apiModel.Title;

            var photo = _context.Photos.Update(entityForUpdate).Entity;
            await _context.SaveChangesAsync();

            var existGenres = await _context.Photos
                .Where(p => p.Id == apiModel.Index)
                .SelectMany(g => g.Genres)
                .Select(g => g.Id)
                .ToListAsync();

            var genresId = apiModel.Genres;

            foreach (var genre in _context.Genres)
            {
                if (genresId.Contains(genre.Id))
                {
                    if (!existGenres.Contains(genre.Id))
                    {
                        photo.Genres.Add(genre);
                    }
                }
                else
                {
                    if (existGenres.Contains(genre.Id))
                    {
                        photo.Genres.Remove(genre);
                    }
                }
            }

            await _context.SaveChangesAsync();

            var photoApi = new PhotoModelAPI
            {
                Index = photo.Id,
                Title = photo.Title,
                Name = photo.Name,
                //Author = _mapper.Map<User>(photo.Author),
                Genres = photo.Genres.Select(g => g.Id).ToArray()
            };

            return Ok(photoApi);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var tryDelete = await _repository.TryDeleteAsync(id);

            if (!tryDelete) return BadRequest();

            return NoContent();
        }

    }
}
