using MovieReservation.Server.Application.Common.Interfaces;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace MovieReservation.Server.Infrastructure.Services
{
    // Dịch vụ Redis để tương tác với cơ sở dữ liệu Redis

    // Cung cấp các phương thức để lưu trữ, truy xuất và quản lý dữ liệu trong Redis

    // Tôi muốn dùng nó để quản lý token nữa
    public class RedisService : IRedisService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _db;
        public RedisService(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _db = _redis.GetDatabase();
        }
        public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(value);
            await _db.StringSetAsync(key, json, expiration);
        }
        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _db.StringGetAsync(key);
            if (value.IsNullOrEmpty) return default;
            return System.Text.Json.JsonSerializer.Deserialize<T>(value);
        }
        public async Task RemoveAsync(string key)
        {
            await _db.KeyDeleteAsync(key);
        }
        public async Task<bool> ExistsAsync(string key)
        {
            return await _db.KeyExistsAsync(key);
        }
        // Lấy RT bằng UserId ($"refresh:{refreshToken}", user.Id,TimeSpan.FromDays(_jwtService.RefreshTokenDays)

        public async Task<string?> GetByUserIdAsync(string userId)
        {
            var server = _redis.GetServer(_redis.GetEndPoints()[0]);
            foreach (var key in server.Keys(pattern: "refresh:*"))
            {
                var value = await _db.StringGetAsync(key);
                if (value == userId)
                {
                    return key.ToString().Replace("refresh:", "");
                }
            }
            return null;
        }
    }
}