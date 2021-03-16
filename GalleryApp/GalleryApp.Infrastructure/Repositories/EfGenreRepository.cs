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
    class EfGenreRepository : IGenreRepository
    {
        private readonly IMapper _mapper;

        public EfGenreRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<ICollection<Genre>> AllGenres()
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

        public async Task<bool> TryCreate(Genre genreForCreate)
        {
            genreForCreate = genreForCreate ?? throw new ArgumentNullException(nameof(genreForCreate));

            bool succses = true;

            using (var context = new GalleryContext())
            {
                var genreEntityExist = await context.Genres.AnyAsync(genreEntity => genreEntity.Name == genreForCreate.Name);

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

        public async Task<bool> TryDelete(Genre modelForDelete)
        {
            modelForDelete = modelForDelete ?? throw new ArgumentNullException(nameof(modelForDelete));

            bool succses = true;

            using (var context = new GalleryContext())
            {
                var genreEntityExist = await context.Genres.AnyAsync(genreEntity => genreEntity.Id == modelForDelete.Index);

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

        public async Task<Genre> GetById(int? id)
        {
            id = id ?? throw new ArgumentNullException(nameof(id));

            Genre result;

            using (var context = new GalleryContext())
            {
                var genreEntityExist = await context.Genres.FirstOrDefaultAsync(genreEntity => genreEntity.Id == id);

                if (genreEntityExist == null)
                    return null;
                else
                {
                    result = _mapper.Map<Genre>(genreEntityExist);
                }
            }

            return result;
        }

        public async Task<bool> TryUpdate(Genre modelForUpdate)
        {
            modelForUpdate = modelForUpdate ?? throw new ArgumentNullException(nameof(modelForUpdate));
            bool succses = true;

            using (var context = new GalleryContext())
            {
                var genreEntityExist = await context.Genres.AnyAsync(genreEntity => genreEntity.Id == modelForUpdate.Index);

                if (!genreEntityExist)
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
    }
}