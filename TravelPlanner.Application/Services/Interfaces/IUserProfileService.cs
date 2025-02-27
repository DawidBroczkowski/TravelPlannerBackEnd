using TravelPlanner.Shared.DTOs.UserProfile;

namespace TravelPlanner.Application.Services.Interfaces
{
    public interface IUserProfileService
    {
        Task CreateProfileAsync(string email, CancellationToken cancellationToken);
        Task<GetUserProfileDto> GetUserProfileAsync(int userId, bool permissionOverride, CancellationToken cancellationToken);
        Task UpdateUserProfileAsync(UpdateUserProfileDto dto, bool permissionOverride, CancellationToken cancellationToken);
        Task AssignProfileImageAsync(Guid fileId, CancellationToken cancellationToken);
    }
}