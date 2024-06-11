using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;
using User.Application.Interfaces.RedisCache;

namespace User.Infrastructure.Redis
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var db = _connectionMultiplexer.GetDatabase();
            var value = await db.StringGetAsync(key);
            if (value.IsNullOrEmpty)
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(value);
        }

        public async Task SetAsync<T>(string key, T value)
        {
            var db = _connectionMultiplexer.GetDatabase();
            var json = JsonConvert.SerializeObject(value);
            await db.StringSetAsync(key, json);
        }
    }

}
