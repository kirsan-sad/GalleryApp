using GalleryApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GalleryApp.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> RegisterAsync(User userForRegister);
        Task<User> LoginAsync(User userForLogin);
    }
}
