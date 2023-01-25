using Microsoft.AspNetCore.Components;
using OnlineStore.Client.Services;
using OnlineStore.Client.Services.Contracts;
using OnlineStore.Models.Dtos;
using System.ComponentModel;

namespace OnlineStore.Client.Pages
{
    public class LoginRegistrationBase: ComponentBase
    {
        [Inject]
        public IUserService UserService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public UserDto User { get; set; }

        public UserDto UserLogin { get; set; }

        public string ErrorMessage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                User = new UserDto()
                {
                    //UserName = "hi",
                    //PasswordHash = "hello"
                };

                UserLogin = new UserDto()
                {
                    //UserName = "hi",
                    //PasswordHash = "hello"
                };
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
        protected async Task RegisterUser_Click(string username, string password)
        {
            try
            {
                var usee = new UserDto()
                {
                    UserName = username,
                    PasswordHash = password
                };
                var userDto = await UserService.RegisterUser(usee);
                NavigationManager.NavigateTo("/");
            }
            catch (Exception)
            {
                
            }
        }

        protected async Task LoginrUser_Click(string username, string password)
        {
            try
            {
                var usee = new UserDto()
                {
                    UserName = username,
                    PasswordHash = password
                };
                await UserService.LoginUser(usee);
                NavigationManager.NavigateTo("/");
            }
            catch (Exception)
            {
            }
        }
    }
}
