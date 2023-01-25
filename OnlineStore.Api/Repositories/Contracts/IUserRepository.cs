using OnlineStore.Api.Entities;
using OnlineStore.Models.Dtos;

namespace OnlineStore.Api.Repositories.Contracts
{
    public interface IUserRepository
    {
        Task<User> RegisterUser(UserDto userToRegister);
        Task<User> GetUser(string username);
    }
}
