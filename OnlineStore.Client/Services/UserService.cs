using OnlineStore.Client.Services.Contracts;
using OnlineStore.Models.Dtos;
using System.Net.Http.Json;

namespace OnlineStore.Client.Services
{
    public class UserService: IUserService
    {

        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserDto> RegisterUser(UserDto userToRegisterDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync<UserDto>("api/LoginReg", userToRegisterDto);

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(UserDto);
                    }

                    return await response.Content.ReadFromJsonAsync<UserDto>();

                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status:{response.StatusCode} Message -{message}");
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
