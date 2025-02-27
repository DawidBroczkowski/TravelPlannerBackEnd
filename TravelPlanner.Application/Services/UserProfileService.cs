using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TravelPlanner.Application.Services.Interfaces;
using TravelPlanner.Infrastructure.Repositories.Interfaces;
using TravelPlanner.Shared.DTOs.UserProfile;

namespace TravelPlanner.Application.Services
{
    public class UserProfileService : BaseService, IUserProfileService
    {
        private readonly IUserProfileRepository _profileRepository;
        private readonly IFileDataRepository _fileDataRepository;
        public UserProfileService(IServiceProvider serviceProvider, IConfiguration configuration) : base(serviceProvider, configuration)
        {
            _profileRepository = serviceProvider.GetRequiredService<IUserProfileRepository>();
            _fileDataRepository = serviceProvider.GetRequiredService<IFileDataRepository>();
        }

        public async Task<GetUserProfileDto> GetUserProfileAsync(int userId, bool permissionOverride, CancellationToken cancellationToken)
        {
            var profile = await _profileRepository.GetUserProfileAsync(userId, cancellationToken);
            if (profile is null)
            {
                var ex = new KeyNotFoundException("User profile not found");
                ex.Data.Add(nameof(userId), userId);
                throw ex;
            }

            if ((profile.UserId != _userId && profile.IsPublic is false) && permissionOverride is false)
            {
                var ex = new UnauthorizedAccessException("You cannot view this user's profile");
                ex.Data.Add(nameof(profile.UserId), profile.UserId);
                throw ex;
            }

            return profile;
        }

        public async Task UpdateUserProfileAsync(UpdateUserProfileDto dto, bool permissionOverride, CancellationToken cancellationToken)
        {
            var profile = await _profileRepository.GetUserProfileAsync(dto.Id, cancellationToken);
            if (profile is null)
            {
                var ex = new KeyNotFoundException("User profile not found");
                ex.Data.Add(nameof(dto.Id), dto.Id);
                throw ex;
            }

            if (profile.UserId != _userId && permissionOverride is false)
            {
                var ex = new UnauthorizedAccessException("You cannot update another user's profile");
                ex.Data.Add(nameof(profile.UserId), profile.UserId);
                throw ex;
            }

            await _profileRepository.UpdateUserProfileAsync(dto, cancellationToken);
        }

        public async Task CreateProfileAsync(string email, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                var ex = new KeyNotFoundException("User with this email does not exist");
                ex.Data.Add(nameof(email), email);
                throw ex;
            }

            var profile = await _profileRepository.GetUserProfileAsync(user.Id, cancellationToken);
            if (profile is not null)
            {
                var ex = new InvalidOperationException("User profile already exists");
                ex.Data.Add(nameof(user.Id), user.Id);
                throw ex;
            }

            await _profileRepository.CreateUserProfileAsync(user.Id, cancellationToken);
        }

        public async Task AssignProfileImageAsync(Guid fileId, CancellationToken cancellationToken)
        {
            var profile = await _profileRepository.GetUserProfileAsync(_userId, cancellationToken);
            if (profile is null)
            {
                var ex = new KeyNotFoundException("User profile not found");
                ex.Data.Add(nameof(_userId), _userId);
                throw ex;
            }

            if (profile.UserId != _userId)
            {
                var ex = new UnauthorizedAccessException("You cannot update another user's profile");
                ex.Data.Add(nameof(profile.UserId), profile.UserId);
                throw ex;
            }

            var fileData = await _fileDataRepository.GetFileDataAsync(fileId, cancellationToken);
            if (fileData is null)
            {
                var ex = new KeyNotFoundException("File not found");
                ex.Data.Add(nameof(fileId), fileId);
                throw ex;
            }

            if (fileData.UploadedById != _userId)
            {
                var ex = new UnauthorizedAccessException("You cannot assign another user's file");
                ex.Data.Add(nameof(fileData.UploadedById), fileData.UploadedById);
                throw ex;
            }

            await _profileRepository.AssignProfileImageAsync(_userId, fileId, cancellationToken);
        }
    }
}
