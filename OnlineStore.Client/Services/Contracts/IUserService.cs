using OnlineStore.Models.Dtos;

namespace OnlineStore.Client.Services.Contracts
{
    public interface IUserService
    {
        Task<UserDto> RegisterUser (UserDto userToRegister);
    }
}
