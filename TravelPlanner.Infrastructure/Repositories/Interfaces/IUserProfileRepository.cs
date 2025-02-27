using TravelPlanner.Shared.DTOs.UserProfile;

namespace TravelPlanner.Infrastructure.Repositories.Interfaces
{
    public interface IUserProfileRepository
    {
        Task AssignFileAsync(int id, int fileDataId, CancellationToken cancellationToken);
        Task CreateUserProfileAsync(int userId, CancellationToken cancellationToken);
        Task DeleteFileAsync(int id, int fileDataId, CancellationToken cancellationToken);
        Task<GetUserProfileDto?> GetUserProfileAsync(int userId, CancellationToken cancellationToken);
        Task<List<GetUserProfileDto>> GetUserProfilesAsync(int from, int to, bool onlyPublic, CancellationToken cancellationToken);
        Task UpdateUserProfileAsync(UpdateUserProfileDto userProfile, CancellationToken cancellationToken);
        Task AssignProfileImageAsync(int userId, Guid fileId, CancellationToken cancellationToken);
    }
}