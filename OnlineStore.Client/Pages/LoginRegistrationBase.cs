using Microsoft.AspNetCore.Components;
using OnlineStore.Client.Services.Contracts;
using OnlineStore.Models.Dtos;

namespace OnlineStore.Client.Pages
{
    public class LoginRegistrationBase: ComponentBase
    {
        [Inject]
        public IUserService UserService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public UserDto User { get; set; } = new UserDto();

        public string ErrorMessage { get; set; }


        protected async Task RegisterUser_Click(UserDto userToRegisterDto)
        {
            try
            {
                var userDto = await UserService.RegisterUser(userToRegisterDto);
                NavigationManager.NavigateTo("/");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
