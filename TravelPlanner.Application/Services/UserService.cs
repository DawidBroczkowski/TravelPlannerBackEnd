using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TravelPlanner.Application.Services.Interfaces;
using TravelPlanner.Infrastructure.Email.Interfaces;
using TravelPlanner.Infrastructure.Repositories.Interfaces;
using TravelPlanner.Shared.DTOs.Penalty;
using TravelPlanner.Shared.DTOs.User;

namespace TravelPlanner.Application.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IJwtBlacklistRepository _jwtBlacklistRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public UserService(IServiceProvider serviceProvider, IConfiguration configuration) : base(serviceProvider, configuration)
        {
            _jwtBlacklistRepository = _serviceProvider.GetRequiredService<IJwtBlacklistRepository>();
            _userRepository = _serviceProvider.GetRequiredService<IUserRepository>();
            _emailService = _serviceProvider.GetRequiredService<IEmailService>();
        }

        public async Task<List<GetPenaltyDto>> GetUserPenaltiesAsync(int userId, CancellationToken cancellationToken)
        {
            return await _userRepository.GetUserPenaltiesAsync(userId, cancellationToken);
        }

        public async Task<GetUserDto?> GetUserAsync(int id, CancellationToken cancellationToken)
        {
            return await _userRepository.GetUserAsync(id, cancellationToken);
        }

        public async Task<GetUserDto?> GetUserAsync(string name, CancellationToken cancellationToken)
        {
            return await _userRepository.GetUserAsync(name, cancellationToken);
        }

        public async Task<GetUserDto?> GetUserByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
        {
            return await _userRepository.GetUserByRefreshTokenAsync(refreshToken, cancellationToken);
        }

        public async Task<GetUserDataDto> GetUserDataAsync(int id, CancellationToken cancellationToken)
        {
            return await _userRepository.GetUserDataAsync(id, cancellationToken);
        }

        public async Task<GetUserDataDto> GetUserDataAsync(string name, CancellationToken cancellationToken)
        {
            return await _userRepository.GetUserDataAsync(name, cancellationToken);
        }

        public async Task<List<GetUserDataDto>> GetUsersDataAsync(int from, int to, CancellationToken cancellationToken)
        {
            return await _userRepository.GetUsersDataAsync(from, to, cancellationToken);
        }

        public async Task<GetUserCredentialsDto> GetUserCredentialsAsync(int id, CancellationToken cancellationToken)
        {
            return await _userRepository.GetUserCredentialsAsync(id, cancellationToken);
        }

        public async Task<GetUserCredentialsDto> GetUserCredentialsByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _userRepository.GetUserCredentialsByEmailAsync(email, cancellationToken);
        }

        public async Task<string?> GetUserRefreshTokenAsync(int id, CancellationToken cancellationToken)
        {
            return await _userRepository.GetUserRefreshTokenAsync(id, cancellationToken);
        }

        public async Task<bool> UserExistsAsync(int id, CancellationToken cancellationToken)
        {
            return await _userRepository.UserExistsAsync(id, cancellationToken);
        }

        public async Task<bool> UserExistsAsync(string name, CancellationToken cancellationToken)
        {
            return await _userRepository.UserExistsAsync(name, cancellationToken);
        }

        public async Task<bool> UserExistsAsync(string name, string email, CancellationToken cancellationToken)
        {
            return await _userRepository.UserExistsAsync(name, email, cancellationToken);
        }

        public async Task<bool> UserExistsByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _userRepository.UserExistsByEmailAsync(email, cancellationToken);
        }

        public async Task UpdateRefreshTokenAsync(int id, string refreshToken, CancellationToken cancellationToken)
        {
            await _userRepository.UpdateRefreshTokenAsync(id, refreshToken, cancellationToken);
        }

        public async Task UpdateTokensAsync(int id, string refreshToken, DateTime refreshTokenExpiry, Guid jti, DateTime jtiExpiry, CancellationToken cancellationToken)
        {
            await _userRepository.UpdateTokensAsync(id, refreshToken, refreshTokenExpiry, jti, jtiExpiry, cancellationToken);
        }
    }
}
