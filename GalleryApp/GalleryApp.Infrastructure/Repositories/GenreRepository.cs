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
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<ICollection<Genre>> GetGenresAsync()
        {
            ICollection<Genre> allGenres;

            var allGenresEntities = from genres in _context.Genres
                                    select genres;

            allGenres = await _mapper.ProjectTo<Genre>(allGenresEntities).ToListAsync();

            return allGenres;
        }

        public async Task<bool> TryCreateAsync(Genre genreForCreate)
        {
            bool success = true;

            var genreEntityExist = await _context.Genres
                .AnyAsync(genreEntity => genreEntity.Name == genreForCreate.Name);

            if (genreEntityExist == true)
                success = false;
            else
            {
                var entityGenre = _mapper.Map<GenreEntity>(genreForCreate);
                _context.Genres.Add(entityGenre);
                await _context.SaveChangesAsync();
            }

            return success;
        }

        public async Task<bool> TryDeleteAsync(int id)
        {
            bool success = true;

            var genreEntityExist = await _context.Genres
                .FirstOrDefaultAsync(genreEntity => genreEntity.Id == id);

            if (genreEntityExist == null)
                success = false;
            else
            {
                var entityForDelete = _mapper.Map<GenreEntity>(genreEntityExist);
                _context.Genres.Remove(entityForDelete);
                await _context.SaveChangesAsync();
            }

            return success;
        }

        public async Task<Genre> GetByIdAsync(int id)
        {
            Genre result;

            var genreEntityExist = await _context.Genres
                .AsNoTracking()
                .Include(p => p.Photos)
                .FirstOrDefaultAsync(genreEntity => genreEntity.Id == id);

            if (genreEntityExist == null)
                return null;
            else
            {
                result = _mapper.Map<Genre>(genreEntityExist);
            }

            return result;
        }

        public async Task<bool> TryUpdateAsync(Genre modelForUpdate)
        {
            bool success = true;

            var genreEntityExist = await _context.Genres
                .AnyAsync(genreEntity => genreEntity.Id == modelForUpdate.Index);

            if (!genreEntityExist)
                success = false;
            else
            {
                var entityForUpdate = _mapper.Map<GenreEntity>(modelForUpdate);
                _context.Genres.Update(entityForUpdate);
                await _context.SaveChangesAsync();
            }

            return success;
        }

        public async Task<int> GetCount()
        {
            return await _context.Genres.AsNoTracking().CountAsync();
        }
    }
}