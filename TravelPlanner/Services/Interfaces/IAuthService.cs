using TravelPlanner.Domain.Models;

namespace TravelPlanner.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(string, Guid, DateTime, long)> CreateAccessTokenAsync(ApplicationUser user, CancellationToken cancellationToken);
        string CreateRefreshToken(out DateTime expires);
    }
}