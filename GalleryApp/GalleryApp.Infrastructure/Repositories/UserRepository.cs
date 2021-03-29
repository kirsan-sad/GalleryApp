using AutoMapper;
using GalleryApp.Domain.Interfaces;
using GalleryApp.Domain.Models;
using GalleryApp.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace GalleryApp.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMapper _mapper;
        private readonly GalleryContext _context;

        public UserRepository(IMapper mapper, GalleryContext context)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<User> LoginAsync(User modelForLogin)
        {
            User result;

            using (var context = _context)
            {
                var userExistsEntityExist = await context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(userEntity => userEntity.Login == modelForLogin.Login);

                result = (userExistsEntityExist == null)? null : result = _mapper.Map<User>(userExistsEntityExist);
            }

            return result;
        }

        public async Task<User> RegisterAsync(User modelForRegister)
        {
            User result;

            using (var context = _context)
            {
                var userExistsEntityExist = await context.Users
                    .FirstOrDefaultAsync(userEntity => userEntity.Login == modelForRegister.Login);
                if (userExistsEntityExist != null)
                    result = null;
                else
                {
                    result = modelForRegister;
                    var entityUser = _mapper.Map<UserEntity>(modelForRegister);
                    context.Users.Add(entityUser);
                    await context.SaveChangesAsync();
                }
            }

            return result;
        }
    }
}
