using GalleryApp.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GalleryApp.Domain.Interfaces
{
    public interface IPhotoRepository : IRepository<Photo>
    {
        Task<ICollection<Photo>> GetAllPhotoByGenreAsync(int? genreId);
        Task<bool> TryUploadAsync(Photo photoForLoading);
        Task<ICollection<Photo>> GetAllPhoto();
    }
}
