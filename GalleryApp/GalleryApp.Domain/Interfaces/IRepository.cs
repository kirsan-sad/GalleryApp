using System.Threading.Tasks;

namespace GalleryApp.Domain.Interfaces
{
    public interface IRepository<TModel> where TModel: class
    {
        Task<TModel> GetById(int? id);
        Task<bool> TryUpdate(TModel modelForUpdate);
        Task<bool> TryDelete(TModel modelForDelete);
    }
}
