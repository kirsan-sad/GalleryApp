using System;
using System.Collections.Generic;
using System.Text;

namespace GalleryApp.Domain.Interfaces
{
    public interface IRepository<TModel> where TModel: class
    {
        TModel GetById(int? id);
        bool TryUpdate(TModel modelForUpdate);
        bool TryDelete(TModel modelForDelete);
    }
}
