using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json.Linq;
using OnlineStore.Client.Services.Contracts;
using OnlineStore.Models.Dtos;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace OnlineStore.Client.Services
{
    public class ProductService: IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly IDistributedCache _cache;
        private readonly ICacheService _cacheService;
        public ProductService(HttpClient httpClient, IDistributedCache cache, ICacheService cacheService)
        {
            _httpClient = httpClient;
            _cache = cache;
            _cacheService = cacheService;   
        }

        public async Task<IEnumerable<ProductDto>> GetItems()
        {
            try
            {
                var token = await _cacheService.GetValueFromKey("jwt_token");

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetAsync("api/Product");
                
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<ProductDto>();
                    }
                    return await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception(message);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ProductDto> GetItem(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Product/{id}");
                
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(ProductDto);
                    }

                    return await response.Content.ReadFromJsonAsync<ProductDto>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception(message);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
