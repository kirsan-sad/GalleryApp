﻿using AutoMapper;
using GalleryApp.Domain.Interfaces;
using GalleryApp.Domain.Models;
using GalleryApp.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalleryApp.Infrastructure.Repositories
{
    public class EfPhotoRepository : IPhotoRepository
    {
        private readonly IMapper _mapper;

        public EfPhotoRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<ICollection<Photo>> GetAllPhotoByGenre(int? genreId)
        {
            genreId = genreId ?? throw new ArgumentNullException(nameof(genreId));

            ICollection<Photo> allPhotos;

            using (var context = new GalleryContext())
            {
                var allPhotoEntitiesByGenre = context.Photos.SelectMany(genre => genre.Genres
                .Where(genreIndex => genreIndex.Id == genreId));

                allPhotos = await _mapper.ProjectTo<Photo>(allPhotoEntitiesByGenre).ToListAsync();
            }

            return allPhotos;
        }

        public async Task<Photo> GetById(int? id)
        {
            id = id ?? throw new ArgumentNullException(nameof(id));

            Photo result;

            using (var context = new GalleryContext())
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

        public async Task<bool> TryDelete(Photo modelForDelete)
        {
            modelForDelete = modelForDelete ?? throw new ArgumentNullException(nameof(modelForDelete));

            bool succses = true;

            using (var context = new GalleryContext())
            {
                var photoEntityExist = await context.Photos.AnyAsync(photoEntity => photoEntity.Id == modelForDelete.Index);

                if (!photoEntityExist)
                    succses = false;
                else
                {
                    var entityForDelete = _mapper.Map<PhotoEntity>(modelForDelete);
                    context.Photos.Remove(entityForDelete);
                    context.SaveChanges();
                }
            }

            return succses;
        }

        public async Task<bool> TryUpdate(Photo modelForUpdate)
        {
            modelForUpdate = modelForUpdate ?? throw new ArgumentNullException(nameof(modelForUpdate));
            bool succses = true;

            using (var context = new GalleryContext())
            {
                var photoEntityExist = await context.Photos.AnyAsync(photoEntity => photoEntity.Id == modelForUpdate.Index);

                if (!photoEntityExist)
                    succses = false;
                else
                {
                    var entityForUpdate = _mapper.Map<GenreEntity>(modelForUpdate);
                    context.Genres.Update(entityForUpdate);
                    context.SaveChanges();
                }
            }

            return succses;
        }

        public async Task<bool> TryUpload(Photo photoForLoading)
        {
            
            throw new NotImplementedException();
        }
    }
}