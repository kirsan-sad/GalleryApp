﻿using GalleryApp.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GalleryApp.Domain.Interfaces
{
    public interface IGenreRepository : IRepository<Genre>
    {
        Task<ICollection<Genre>> AllGenres();
        Task<bool> TryCreate(Genre genreForCreate);
    }
}
