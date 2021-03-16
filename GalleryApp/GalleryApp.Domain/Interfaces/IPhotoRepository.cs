using GalleryApp.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GalleryApp.Domain.Interfaces
{
    public interface IPhotoRepository : IRepository<Photo>
    {
        Task<ICollection<Photo>> GetAllPhotoByGenre(int? genreId);
        Task<bool> TryUpload(Photo photoForLoading);
    }
}
