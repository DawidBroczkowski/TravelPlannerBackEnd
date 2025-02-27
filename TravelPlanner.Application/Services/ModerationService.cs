using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using TravelPlanner.Application.Services.Interfaces;
using TravelPlanner.Domain.Models;
using TravelPlanner.Infrastructure.Repositories.Interfaces;
using TravelPlanner.Shared.DTOs.Penalty;

namespace TravelPlanner.Application.Services
{
    public class ModerationService : BaseService, IModerationService
    {
        private IUserRepository _userRepository;
        private IJwtBlacklistRepository _jwtBlacklistRepository;

        public ModerationService(IServiceProvider serviceProvider, IConfiguration configuration) : base(serviceProvider, configuration)
        {
            _userRepository = _serviceProvider.GetRequiredService<IUserRepository>();
            _jwtBlacklistRepository = _serviceProvider.GetRequiredService<IJwtBlacklistRepository>();
        }

        public async Task ApplyPenaltyToUserAsync(CreatePenaltyDto dto, CancellationToken cancellationToken)
        {
            int.TryParse(_userContext.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out int issuerId);
            var user = await _userManager.FindByIdAsync(dto.UserId.ToString());
            if (user is null)
            {
                var ex = new ValidationException("User with this id does not exist");
                ex.Data.Add(nameof(dto.UserId), dto.UserId);
                throw ex;
            }

            var issuer = await _userManager.FindByIdAsync(issuerId.ToString());
            Permission userPermissions = await user.GetUserPermissionsAsync(_serviceProvider, cancellationToken);
            Permission issuerPermissions = await issuer!.GetUserPermissionsAsync(_serviceProvider, cancellationToken);

            if (userPermissions >= issuerPermissions)
            {
                var ex = new UnauthorizedAccessException("You cannot apply a penalty to a user with greater or equal permissions");
                ex.Data.Add(nameof(dto.UserId), dto.UserId);
                throw ex;
            }

            switch (dto.Type)
            {
                case PenaltyType.AccountBan:
                    await _jwtBlacklistRepository.BlacklistTokenAsync(user!.Jti.ToString()!, user.JtiExpiry!.Value.AddMinutes(1));
                    break;
                case PenaltyType.Mute:
                    await _jwtBlacklistRepository.BlacklistTokenAsync(user!.Jti.ToString()!, user.JtiExpiry!.Value.AddMinutes(1));
                    break;
                case PenaltyType.Warning:
                    break;
                default:
                    break;
            }

            await _userRepository.AddPenaltyToUserAsync(dto, issuerId, cancellationToken);
        }
    }
}
