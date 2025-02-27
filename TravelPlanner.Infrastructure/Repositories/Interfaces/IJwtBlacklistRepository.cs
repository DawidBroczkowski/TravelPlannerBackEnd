namespace TravelPlanner.Infrastructure.Repositories.Interfaces
{
    public interface IJwtBlacklistRepository
    {
        Task BlacklistTokenAsync(string jti, DateTime expiration);
        Task<bool> IsTokenBlacklistedAsync(string jti);
        Task<string> GetBlacklistedTokensAsync();
    }
}