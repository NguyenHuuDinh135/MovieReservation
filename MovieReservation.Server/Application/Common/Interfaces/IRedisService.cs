using StackExchange.Redis;

namespace MovieReservation.Server.Application.Common.Interfaces
{
    public interface IRedisService
    {
        Task SetAsync<T>(string key, T value, TimeSpan expiration);
        Task<T?> GetAsync<T>(string key);
        // Get RT bằng UserId
        Task<string?> GetByUserIdAsync(string userId);
        Task RemoveAsync(string key);
        Task<bool> ExistsAsync(string key);
    }
}