using System.Threading.Tasks;

namespace GalleryApp.Domain.Interfaces
{
    public interface IRepository<TModel> where TModel: class
    {
        Task<TModel> GetByIdAsync(int id);
        //Task<bool> TryUpdateAsync(TModel modelForUpdate);
        Task<bool> TryDeleteAsync(int id);
        Task<int> GetCount();
    }
}
