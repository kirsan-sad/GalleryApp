using GalleryApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GalleryApp.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<ICollection<User>> GetUsersAsync();
        Task<User> RegisterAsync(User userForRegister);
        Task<User> LoginAsync(User userForLogin);
        Task<bool> TryUpdateAsync(User userForUpdate);
        Task<bool> TryCreateAsync(User userForCreate);
    }
}
