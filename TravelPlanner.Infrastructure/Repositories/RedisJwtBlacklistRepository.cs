using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using TravelPlanner.Infrastructure.Repositories.Interfaces;

namespace TravelPlanner.Infrastructure.Repositories
{
    public class RedisJwtBlacklistRepository : IJwtBlacklistRepository
    {
        private readonly StackExchange.Redis.IDatabase _redisDatabase;
        private readonly IServiceProvider _serviceProvider;

        public RedisJwtBlacklistRepository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _redisDatabase = _serviceProvider.GetRequiredService<IConnectionMultiplexer>().GetDatabase();
        }

        // Add token to the blacklist
        public async Task BlacklistTokenAsync(string jti, DateTime expiration)
        {
            var ttl = expiration - DateTime.UtcNow;
            await _redisDatabase.StringSetAsync(jti, "blacklisted", ttl);
        }

        // Check if token is blacklisted
        public async Task<bool> IsTokenBlacklistedAsync(string jti)
        {
            return await _redisDatabase.KeyExistsAsync(jti);
        }

        public async Task<string> GetBlacklistedTokensAsync()
        {
            var keys = await _redisDatabase.ExecuteAsync("KEYS", "*");
            return keys.ToString();
        }
    }
}
