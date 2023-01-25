using OnlineStore.Api.Entities;
using OnlineStore.Api.Data;
using OnlineStore.Api.Repositories.Contracts;
using OnlineStore.Models.Dtos;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace OnlineStore.Api.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly OnlineStoreDbContext _onlineStoreDbContext;

        public UserRepository(OnlineStoreDbContext onlineStoreDbContext)
        {
            _onlineStoreDbContext = onlineStoreDbContext;
        }

        public async Task<User> RegisterUser(User userToRegister)
        {
            var isUserExist = await UserExists(userToRegister.UserName);
            if (!isUserExist)
            {
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                userToRegister.PasswordHash = Hasher.HashPassword(userToRegister, userToRegister.PasswordHash);


                var result = await _onlineStoreDbContext.Users.AddAsync(userToRegister);
                await _onlineStoreDbContext.SaveChangesAsync();
                return result.Entity;
            }

            return null;
        }

        private async Task<bool> UserExists(string username)
        {
            return await _onlineStoreDbContext.Users.AnyAsync(c => c.UserName == username);
        }
    }
}
