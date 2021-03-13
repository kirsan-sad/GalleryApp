using GalleryApp.Domain.Interfaces;
using GalleryApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GalleryApp.Infrastructure.Repositories
{
    class EfGenreRepository : IGenreRepository
    {
        public ICollection<Genre> AllGenres()
        {
            throw new NotImplementedException();
        }

        public bool TryCreate(Genre genreForCreate)
        {
            throw new NotImplementedException();
        }

        public bool TryDelete(Genre modelForDelete)
        {
            throw new NotImplementedException();
        }

        public bool TryRead(Genre modelForRead)
        {
            throw new NotImplementedException();
        }

        public bool TryUpdate(Genre modelForUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
