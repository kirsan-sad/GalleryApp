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

            var allPhotoEntitiesByGenre = _context.Genres.Where(genreIndex => genreIndex.Id == genreId)
                .SelectMany(photos => photos.Photos);

            allPhotos = await _mapper.ProjectTo<Photo>(allPhotoEntitiesByGenre).ToListAsync();

            return allPhotos;
        }

        public async Task<Photo> GetByIdAsync(int id)
        {
            Photo result;

            var photoEntityExist = await _context.Photos.Include(g => g.Genres)
                .FirstOrDefaultAsync(photoEntity => photoEntity.Id == id);

            if (photoEntityExist == null)
                return null;
            else
            {
                result = _mapper.Map<Photo>(photoEntityExist);
            }

            return result;
        }

        public async Task<bool> TryDeleteAsync(int id)
        {
            bool success = true;

            var photoEntityExist = await _context.Photos
                .FirstOrDefaultAsync(photoEntity => photoEntity.Id == id);

            if (photoEntityExist == null)
                success = false;
            else
            {
                var entityForDelete = _mapper.Map<PhotoEntity>(photoEntityExist);
                _context.Photos.Remove(entityForDelete);
                await _context.SaveChangesAsync();
            }

            return success;
        }

        public async Task<bool> TryUpdateAsync(Photo modelForUpdate, List<int> genresId)
        {
            bool success = true;

            var photoEntityExist = await _context.Photos
                .AnyAsync(photoEntity => photoEntity.Id == modelForUpdate.Index);

            if (!photoEntityExist)
                success = false;
            else
            {
                var entityForUpdate = _mapper.Map<PhotoEntity>(modelForUpdate);
                var photo = _context.Photos.Update(entityForUpdate).Entity;
                await _context.SaveChangesAsync();

                photo.Genres = _context.Genres
                    .Where(genre => genresId.Contains(genre.Id)).ToList();
                _context.Update(photo);
                //await _context.SaveChangesAsync();
                _context.SaveChanges();
                //_context.Photos.Update(entityForUpdate);
                //await _context.SaveChangesAsync();
            }

            return success;
        }

        public async Task<bool> TryUploadAsync(Photo photoForUploading, List<int> genresId)
        {
            bool success = true;

            var photoEntityExist = await _context.Photos
                .AnyAsync(photoEntity => photoEntity.Name == photoForUploading.Name);

            if (photoEntityExist == true) //photoEntityExists, проверить жанр
                success = false;
            else
            {
                var entityPhoto = _mapper.Map<PhotoEntity>(photoForUploading);
                var photo = _context.Photos.Add(entityPhoto).Entity;
                await _context.SaveChangesAsync();

                photo.Genres = _context.Genres
                    .Where(genre => genresId.Contains(genre.Id)).ToList();
                _context.Update(photo);
                await _context.SaveChangesAsync();

                success = true;
            }

            return success;
        }

        public async Task<ICollection<Photo>> GetPhotosAsync()
        {
            ICollection<Photo> allPhotos;

            var allPhotoEntities = from Photo in _context.Photos
                                   select Photo;

            allPhotos = await _mapper.ProjectTo<Photo>(allPhotoEntities).ToListAsync();

            return allPhotos;
        }

        public async Task<ICollection<LastPhotos>> GetLastPhotosAsync(int numberOfPhotos)
        {
            ICollection<LastPhotos> lastPhotos;

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

            return lastPhotos;
        }

        public async Task<int> GetCount()
        {
            return await _context.Photos.AsNoTracking().CountAsync();
        }
    }
}
