using GalleryApp.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GalleryApp.Domain.Interfaces
{
    public interface IGenreRepository : IRepository<Genre>
    {
        Task<ICollection<Genre>> GetGenresAsync();
        Task<bool> TryCreateAsync(Genre genreForCreate);
        Task<bool> TryUpdateAsync(Genre genreForUpdate);
    }
}
