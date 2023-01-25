using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;
using OnlineStore.Client.Services.Contracts;
using OnlineStore.Models.Dtos;
using System.Net.Http.Json;
using System.Text;

namespace OnlineStore.Client.Services
{
    public class UserService: IUserService
    {

        private readonly HttpClient _httpClient;
        private readonly IDistributedCache _cache;
        private readonly ICacheService _cacheService;

        public UserService(HttpClient httpClient, IDistributedCache cache, ICacheService cacheService)
        {
            _httpClient = httpClient;
            _cache = cache;
            _cacheService = cacheService;
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

        public async Task LoginUser(UserDto userToLogin)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync<UserDto>("api/LoginReg/login", userToLogin);

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return;
                    }
                    var res = await response.Content.ReadAsStringAsync();
                    _cacheService.SetKey("jwt_token", res);
                    return;
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
