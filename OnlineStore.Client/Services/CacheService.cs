using Microsoft.Extensions.Caching.Distributed;
using OnlineStore.Client.Services.Contracts;
using System.Text;

namespace OnlineStore.Client.Services
{
    public class CacheService: ICacheService
    {
        private readonly IDistributedCache _cache;

        public CacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task SetKey(string key, string value)
        {
            byte[] byteValue = Encoding.UTF8.GetBytes(value);
            var options = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) };
            await _cache.SetAsync(key, byteValue, options);
        }

        public async Task<string> GetValueFromKey(string key)
        {
            byte[] byteres = await _cache.GetAsync(key);
            return Encoding.UTF8.GetString(byteres);
        }
    }
}
