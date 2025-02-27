using TravelPlanner.Shared.DTOs.Penalty;
using TravelPlanner.Shared.DTOs.User;

namespace TravelPlanner.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<GetUserDto?> GetUserAsync(int id, CancellationToken cancellationToken);
        Task<List<GetPenaltyDto>> GetUserPenaltiesAsync(int userId, CancellationToken cancellationToken);
        Task<GetUserDto?> GetUserAsync(string name, CancellationToken cancellationToken);
        Task<GetUserDto?> GetUserByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
    }
}