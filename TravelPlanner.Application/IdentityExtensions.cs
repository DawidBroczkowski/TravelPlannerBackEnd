using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TravelPlanner.Domain.Models;
using TravelPlanner.Infrastructure.Repositories.Interfaces;
using TravelPlanner.Shared.DTOs.User;

namespace TravelPlanner.Application
{
    public static class IdentityExtensions
    {
        public async static Task<GetUserDto> AsGetUserDto(this ApplicationUser user, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var userRepository = serviceProvider.GetRequiredService<IUserRepository>();
            var penalties = await userRepository.GetUserPenaltiesAsync(user.Id, cancellationToken);

            var userDto = new GetUserDto()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email!,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                LastLogin = user.LastLogin,
                RefreshToken = user.RefreshToken,
                RefreshTokenExpiry = user.RefreshTokenExpiry,
                Jti = user.Jti,
                JtiExpiry = user.JtiExpiry
            };

            userDto.IsBanned = penalties.Any(p => !p.IsDeleted && (p.IsPermanent || (p.StartDate <= DateTime.UtcNow && DateTime.UtcNow <= p.EndDate)) && p.Type == PenaltyType.AccountBan);
            userDto.IsMuted = penalties.Any(p => !p.IsDeleted && (p.IsPermanent || (p.StartDate <= DateTime.UtcNow && DateTime.UtcNow <= p.EndDate)) && p.Type == PenaltyType.Mute);
            userDto.MuteEndDate = penalties
                .Where(p => !p.IsDeleted && p.Type == PenaltyType.Mute && (p.IsPermanent || (p.StartDate <= DateTime.UtcNow && DateTime.UtcNow <= p.EndDate)))
                .OrderByDescending(p => p.EndDate) // Get the most relevant mute penalty
                .Select(p => p.IsPermanent ? (DateTime?)null : p.EndDate) // If permanent, return null; otherwise, return EndDate
                .FirstOrDefault(); // Get the first (if any) mute end date

            return userDto;
        }

        public static async Task<Permission> GetUserPermissionsAsync(this ApplicationUser user, IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roles = await userManager.GetRolesAsync(user);

            if (roles == null || !roles.Any())
            {
                return Permission.None;
            }

            Permission permissions = Permission.None;
            foreach (var role in roles)
            {
                var roleEntity = await roleManager.FindByNameAsync(role);
                if (roleEntity != null)
                {
                    var roleClaims = await roleManager.GetClaimsAsync(roleEntity);
                    foreach (var claim in roleClaims)
                    {
                        if (claim.Type == "permissions" && Enum.TryParse<Permission>(claim.Value, out var permission))
                        {
                            permissions |= permission;
                        }
                    }
                }
            }

            return permissions;
        }
    }
}
