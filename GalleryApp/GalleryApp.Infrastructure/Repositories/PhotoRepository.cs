using AutoMapper;
using AutoMapper.QueryableExtensions;
using GalleryApp.Domain.Interfaces;
using GalleryApp.Domain.Models;
using GalleryApp.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace GalleryApp.Infrastructure.Repositories
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly IMapper _mapper;
        private readonly GalleryContext _context;

        public PhotoRepository(IMapper mapper, GalleryContext context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<ICollection<Photo>> GetPhotosByGenreAsync(int genreId)
        {
            ICollection<Photo> allPhotos;

            using (var context = _context)
            {
                var allPhotoEntitiesByGenre = context.Genres.Where(genreIndex => genreIndex.Id == genreId)
                    .SelectMany(photos => photos.Photos);

                allPhotos = await _mapper.ProjectTo<Photo>(allPhotoEntitiesByGenre).ToListAsync();
            }

            return allPhotos;
        }

        public async Task<Photo> GetByIdAsync(int id)
        {
            Photo result;

            using (var context = _context)
            {

                var photoEntityExist = await context.Photos.FirstOrDefaultAsync(photoEntity => photoEntity.Id == id);

                if (photoEntityExist == null)
                    return null;
                else
                {
                    result = _mapper.Map<Photo>(photoEntityExist);
                }
            }

            return result;
        }

        public async Task<bool> TryDeleteAsync(Photo modelForDelete)
        {
            bool success = true;

            using (var context = _context)
            {
                var photoEntityExist = await context.Photos.AnyAsync(photoEntity => photoEntity.Id == modelForDelete.Index);

                if (!photoEntityExist)
                    success = false;
                else
                {
                    var entityForDelete = _mapper.Map<PhotoEntity>(modelForDelete);
                    context.Photos.Remove(entityForDelete);
                    await context.SaveChangesAsync();
                }
            }

            return success;
        }

        public async Task<bool> TryUpdateAsync(Photo modelForUpdate)
        {
            bool success = true;

            using (var context = _context)
            {
                var photoEntityExist = await context.Photos.AnyAsync(photoEntity => photoEntity.Id == modelForUpdate.Index);

                if (!photoEntityExist)
                    success = false;
                else
                {
                    var entityForUpdate = _mapper.Map<PhotoEntity>(modelForUpdate);
                    context.Photos.Update(entityForUpdate);
                    await context.SaveChangesAsync();
                }
            }

            return success;
        }

        public async Task<bool> TryUploadAsync(Photo photoForUpoading, List<int> genresId)
        {
            bool success = true;

            using (var context = _context)
            {
                var photoEntityExist = await context.Photos.AnyAsync(photoEntity => photoEntity.Name == photoForUpoading.Name);

                if (photoEntityExist == true) //photoEntityExists, проверить жанр
                    success = false;
                else
                {
                    var entityPhoto = _mapper.Map<PhotoEntity>(photoForUpoading);
                    var photo = context.Photos.Add(entityPhoto).Entity;
                    await context.SaveChangesAsync();

                    photo.Genres = context.Genres.Where(genre => genresId.Contains(genre.Id)).ToList();
                    context.Update(photo);
                    await context.SaveChangesAsync();

                    success = true;
                }
            }

            return success;
        }

        public async Task<ICollection<Photo>> GetPhotos()
        {
            ICollection<Photo> allPhotos;

            using (var context = _context)
            {
                var allPhotoEntities = from Photo in context.Photos
                                       select Photo;

                allPhotos = await _mapper.ProjectTo<Photo>(allPhotoEntities).ToListAsync();
            }

            return allPhotos;
        }

        public async Task<ICollection<LastPhotos>> GetLastPhotosAsync(int numberOfPhotos)
        {
            ICollection<LastPhotos> lastPhotos;

            using (var context = _context)
            {
                lastPhotos = await _context.Genres
                    .AsNoTracking()
                    .SelectMany(genre => genre.Photos.Take(1),
                    (genre, photo)
                    => new LastPhotos
                    {
                        GenreId = genre.Id,
                        Genre = genre.Name,
                        Photos = _mapper.Map<IEnumerable<Photo>>(genre.Photos.OrderByDescending(x => x.Id).Take(numberOfPhotos)).AsEnumerable()
                        .ToList()
                    }).ToListAsync();
            }
           
            return lastPhotos;
        }
    }
}
