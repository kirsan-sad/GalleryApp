using GalleryApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GalleryApp.Domain.Interfaces
{
    public interface IGenreRepository : IRepository<Genre>
    {
        ICollection<Genre> AllGenres();
        bool TryCreate(Genre genreForCreate);
    }
}
