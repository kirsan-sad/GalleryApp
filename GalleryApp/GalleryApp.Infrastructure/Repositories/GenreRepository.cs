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
    public class GenreRepository : IGenreRepository
    {
        private readonly IMapper _mapper;
        private readonly GalleryContext _context;

        public GenreRepository(IMapper mapper, GalleryContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ICollection<Genre>> GetGenresAsync()
        {
            ICollection<Genre> allGenres;

            using (var context = _context)
            {
                var allGenresEntities = from genres in context.Genres
                                        select genres;

                allGenres = await _mapper.ProjectTo<Genre>(allGenresEntities).ToListAsync();
            }

            return allGenres;
        }

        public async Task<bool> TryCreateAsync(Genre genreForCreate)
        {
            bool success = true;

            using (var context = _context)
            {
                var genreEntityExist = await context.Genres
                    .AnyAsync(genreEntity => genreEntity.Name == genreForCreate.Name);

                if (genreEntityExist == true)
                    success = false;
                else
                {
                    var entityGenre = _mapper.Map<GenreEntity>(genreForCreate);
                    context.Genres.Add(entityGenre);
                    await context.SaveChangesAsync();
                }
            }

            return success;
        }

        public async Task<bool> TryDeleteAsync(Genre modelForDelete)
        {
            bool success = true;

            using (var context = _context)
            {
                var genreEntityExist = await context.Genres
                    .AnyAsync(genreEntity => genreEntity.Id == modelForDelete.Index);

                if (!genreEntityExist)
                    success = false;
                else
                {
                    var entityForDelete = _mapper.Map<GenreEntity>(modelForDelete);
                    context.Genres.Remove(entityForDelete);
                    await context.SaveChangesAsync();
                }
            }

            return success;
        }

        public async Task<Genre> GetByIdAsync(int id)
        {
            Genre result;

            using (var context = _context)
            {
                var genreEntityExist = await context.Genres
                    .AsNoTracking()
                    .FirstOrDefaultAsync(genreEntity => genreEntity.Id == id);

                genreEntityExist = genreEntityExist ?? throw new ArgumentNullException(nameof(genreEntityExist));

                result = _mapper.Map<Genre>(genreEntityExist);
            }

            return result;
        }

        public async Task<bool> TryUpdateAsync(Genre modelForUpdate)
        {
            bool success = true;

            using (var context = _context)
            {
                var genreEntityExist = await context.Genres
                    .AnyAsync(genreEntity => genreEntity.Id == modelForUpdate.Index);

                if (!genreEntityExist)
                    success = false;
                else
                {
                    var entityForUpdate = _mapper.Map<GenreEntity>(modelForUpdate);
                    context.Genres.Update(entityForUpdate);
                    await context.SaveChangesAsync();
                }
            }

            return success;
        }
    }
}