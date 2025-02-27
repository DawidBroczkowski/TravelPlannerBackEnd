using TravelPlanner.Shared.DTOs.Penalty;
using TravelPlanner.Shared.DTOs.User;

namespace TravelPlanner.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<GetUserDto?> GetUserAsync(int id, CancellationToken cancellationToken);
        Task<GetUserDto?> GetUserAsync(string name, CancellationToken cancellationToken);
        Task<GetUserDto?> GetUserByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
        Task<List<GetUserDto>> GetUsersAsync(int from, int to, CancellationToken cancellationToken);
        Task<GetUserDataDto> GetUserDataAsync(int id, CancellationToken cancellationToken);
        Task<GetUserDataDto> GetUserDataAsync(string name, CancellationToken cancellationToken);
        Task<List<GetUserDataDto>> GetUsersDataAsync(int from, int to, CancellationToken cancellationToken);
        Task<GetUserDto?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
        Task<GetUserCredentialsDto> GetUserCredentialsAsync(int id, CancellationToken cancellationToken);
        Task<GetUserCredentialsDto> GetUserCredentialsByEmailAsync(string email, CancellationToken cancellationToken);
        Task<string?> GetUserRefreshTokenAsync(int id, CancellationToken cancellationToken);
        Task<bool> UserExistsAsync(int id, CancellationToken cancellationToken);
        Task<bool> UserExistsAsync(string name, CancellationToken cancellationToken);
        Task<bool> UserExistsAsync(string name, string email, CancellationToken cancellationToken);
        Task<bool> UserExistsByEmailAsync(string email, CancellationToken cancellationToken);
        Task UpdateRefreshTokenAsync(int id, string refreshToken, CancellationToken cancellationToken);
        Task UpdateTokensAsync(int id, string refreshToken, DateTime refreshTokenExpiry, Guid jti, DateTime jtiExpiry, CancellationToken cancellationToken);
        //Task AddUserAsync(CreateUserDto dto, string roleName, CancellationToken cancellationToken);
        Task AddPenaltyToUserAsync(CreatePenaltyDto penaltyDto, int issuerId, CancellationToken cancellationToken);
        Task<List<GetPenaltyDto>> GetUserPenaltiesAsync(int userId, CancellationToken cancellationToken);
    }
}