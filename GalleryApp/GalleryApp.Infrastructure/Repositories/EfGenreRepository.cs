using AutoMapper;
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
    public class EfGenreRepository : IGenreRepository
    {
        private readonly IMapper _mapper;

        public EfGenreRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<ICollection<Genre>> AllGenresAsync()
        {
            ICollection<Genre> allGenres;

            using (var context = new GalleryContext())
            {
                var allGenresEntities = from genres in context.Genres
                                      select genres;

                allGenres = await _mapper.ProjectTo<Genre>(allGenresEntities).ToListAsync();
            }

            return allGenres;
        }

        public async Task<bool> TryCreateAsync(Genre genreForCreate)
        {
            bool succses = true;

            using (var context = new GalleryContext())
            {
                var genreEntityExist = await context.Genres
                    .AnyAsync(genreEntity => genreEntity.Name == genreForCreate.Name);

                if (genreEntityExist == true)
                    succses = false;
                else
                {
                    var entityGenre = _mapper.Map<GenreEntity>(genreForCreate);
                    context.Genres.Add(entityGenre);
                    await context.SaveChangesAsync();
                }
            }

            return succses;
        }

        public async Task<bool> TryDeleteAsync(Genre modelForDelete)
        {
            bool succses = true;

            using (var context = new GalleryContext())
            {
                var genreEntityExist = await context.Genres
                    .AnyAsync(genreEntity => genreEntity.Id == modelForDelete.Index);

                if (!genreEntityExist)
                    succses = false;
                else
                {
                    var entityForDelete = _mapper.Map<GenreEntity>(modelForDelete);
                    context.Genres.Remove(entityForDelete);
                    await context.SaveChangesAsync();
                }
            }

            return succses;
        }

        public async Task<Genre> GetByIdAsync(int? id)
        {
            Genre result;

            using (var context = new GalleryContext())
            {
                var genreEntityExist = await context.Genres
                    .AsNoTracking()
                    .FirstOrDefaultAsync(genreEntity => genreEntity.Id == id);

                if (genreEntityExist == null)
                    return null;
                else
                {
                    result = _mapper.Map<Genre>(genreEntityExist);
                }
            }

            return result;
        }

        public async Task<bool> TryUpdateAsync(Genre modelForUpdate)
        {
            bool succses = true;

            using (var context = new GalleryContext())
            {
                var genreEntityExist = await context.Genres
                    .AnyAsync(genreEntity => genreEntity.Id == modelForUpdate.Index);

                if (!genreEntityExist)
                    succses = false;
                else
                {
                    var entityForUpdate = _mapper.Map<GenreEntity>(modelForUpdate);
                    context.Genres.Update(entityForUpdate);
                    await context.SaveChangesAsync();
                }
            }

            return succses;
        }
    }
}