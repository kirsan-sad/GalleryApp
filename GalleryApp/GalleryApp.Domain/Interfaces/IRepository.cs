using System;
using System.Collections.Generic;
using System.Text;

namespace GalleryApp.Domain.Interfaces
{
    public interface IRepository<TModel> where TModel: class
    {
        bool TryRead(TModel modelForRead);
        bool TryUpdate(TModel modelForUpdate);
        bool TryDelete(TModel modelForDelete);
    }
}
