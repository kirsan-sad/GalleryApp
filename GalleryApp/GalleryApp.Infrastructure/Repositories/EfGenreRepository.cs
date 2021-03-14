using AutoMapper;
using GalleryApp.Domain.Interfaces;
using GalleryApp.Domain.Models;
using GalleryApp.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GalleryApp.Infrastructure.Repositories
{
    class EfGenreRepository : IGenreRepository
    {
        private readonly IMapper _mapper;

        public EfGenreRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public ICollection<Genre> AllGenres()
        {
            ICollection<Genre> allGenres;

            using (var context = new GalleryContext())
            {
                var allGenresEntity = from genres in context.Genres
                                      select genres;

                allGenres = _mapper.ProjectTo<Genre>(allGenresEntity).ToList();
            }

            return allGenres;
        }

        public bool TryCreate(Genre genreForCreate)
        {
            genreForCreate = genreForCreate ?? throw new ArgumentNullException(nameof(genreForCreate));

            bool succses = true;

            using (var context = new GalleryContext())
            {
                var genreEntityExist = context.Genres.Any(genreEntity => genreEntity.Name == genreForCreate.Name);

                if (genreEntityExist == true)
                    succses = false;
                else
                {
                    var entityGenre = _mapper.Map<GenreEntity>(genreForCreate);
                    context.Genres.Add(entityGenre);
                    context.SaveChanges();
                }
            }

            return succses;
        }

        public bool TryDelete(Genre modelForDelete)
        {
            modelForDelete = modelForDelete ?? throw new ArgumentNullException(nameof(modelForDelete));

            bool succses = true;

            using (var context = new GalleryContext())
            {
                var genreEntityExist = context.Genres.Any(genreEntity => genreEntity.Id == modelForDelete.Index);

                if (!genreEntityExist)
                    succses = false;
                else
                {
                    var entityForDelete = _mapper.Map<GenreEntity>(modelForDelete);
                    context.Remove(entityForDelete);
                    context.SaveChanges();
                }
            }

            return succses;
        }

        public Genre GetById(int? id)
        {
            id = id ?? throw new ArgumentNullException(nameof(id));

            Genre result;

            using (var context = new GalleryContext())
            {
                var genreEntityExist = context.Genres.FirstOrDefault(genreEntity => genreEntity.Id == id);

                if (genreEntityExist == null)
                    return null;
                else
                {
                    result = _mapper.Map<Genre>(genreEntityExist);
                }
            }

            return result;
        }

        public bool TryUpdate(Genre modelForUpdate)
        {
            modelForUpdate = modelForUpdate ?? throw new ArgumentNullException(nameof(modelForUpdate));
            bool succses = true;

            using (var context = new GalleryContext())
            {
                var genreEntityExist = context.Genres.Any(genreEntity => genreEntity.Id == modelForUpdate.Index);

                if (!genreEntityExist)
                    succses = false;
                else
                {
                    var entityForUpdate = _mapper.Map<GenreEntity>(modelForUpdate);
                    context.Update(entityForUpdate);
                    context.SaveChanges();
                }
            }

            return succses;
        }
    }
}