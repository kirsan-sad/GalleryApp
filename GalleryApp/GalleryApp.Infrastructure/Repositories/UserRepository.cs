using AutoMapper;
using GalleryApp.Domain.Interfaces;
using GalleryApp.Domain.Models;
using GalleryApp.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

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

        public async Task<User> GetByIdAsync(int id)
        {
            User result;

            var userEntityExist = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(userEntity => userEntity.Id == id);

            userEntityExist = userEntityExist ?? throw new ArgumentNullException(nameof(userEntityExist));

            result = _mapper.Map<User>(userEntityExist);

            return result;
        }

        public async Task<int> GetCount()
        {
            return await _context.Users.AsNoTracking().CountAsync();
        }

        public async Task<ICollection<User>> GetUsersAsync()
        {
            ICollection<User> allUsers;

            var allUsersEntities = from users in _context.Users
                                    select users;

            allUsers = await _mapper.ProjectTo<User>(allUsersEntities).ToListAsync();

            return allUsers;
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

        public async Task<bool> TryCreateAsync(User userForCreate)
        {
            bool success = true;

            var userEntityExist = await _context.Users
                .AnyAsync(userEntity => userEntity.Login == userForCreate.Login);

            if (userEntityExist == true)
                success = false;
            else
            {
                var entityUser = _mapper.Map<UserEntity>(userForCreate);
                _context.Users.Add(entityUser);
                await _context.SaveChangesAsync();
            }

            return success;
        }

        public async Task<bool> TryDeleteAsync(int id)
        {
            bool success = true;

            var userEntityExist = await _context.Users
                .FirstOrDefaultAsync(userEntity => userEntity.Id == id);

            if (userEntityExist == null)
                success = false;
            else
            {
                var entityForDelete = _mapper.Map<UserEntity>(userEntityExist);
                _context.Users.Remove(entityForDelete);
                await _context.SaveChangesAsync();
            }

            return success;
        }

        public async Task<bool> TryUpdateAsync(User modelForUpdate)
        {
            bool success = true;

            var userEntityExist = await _context.Users
                .AnyAsync(userEntity => userEntity.Id == modelForUpdate.Index);

            if (!userEntityExist)
                success = false;
            else
            {
                var entityForUpdate = _mapper.Map<UserEntity>(modelForUpdate);
                _context.Users.Update(entityForUpdate);
                await _context.SaveChangesAsync();
            }

            return success;
        }
    }
}
