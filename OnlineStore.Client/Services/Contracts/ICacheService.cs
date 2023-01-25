namespace OnlineStore.Client.Services.Contracts
{
    public interface ICacheService
    {
        Task SetKey(string key, string value);
        Task<string> GetValueFromKey(string key);
    }
}
