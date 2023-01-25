using OnlineStore.Api.Entities;

namespace OnlineStore.Api.Repositories.Contracts
{
    public interface IUserRepository
    {
        Task<User> RegisterUser(User userToRegister);
    }
}
