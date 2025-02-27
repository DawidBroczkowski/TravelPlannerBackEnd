using TravelPlanner.Shared.DTOs.Auth;
using TravelPlanner.Shared.DTOs.User;

namespace TravelPlanner.Services.Interfaces
{
    public interface IAccountService
    {
        Task ChangePasswordAsync(ChangePasswordDto dto);
        Task ConfirmEmailAsync(string email, string token);
        Task ForgotPasswordAsync(string email, string passwordResetRoute);
        Task<AuthenticationDto> LoginUserAsync(LoginUserDto dto, CancellationToken cancellationToken);
        Task LogoutUserAsync(CancellationToken cancellationToken);
        Task<AuthenticationDto> RefreshTokenAsync(RefreshTokenDto dto, CancellationToken cancellationToken);
        Task<string?> RegisterUserAsync(RegisterUserDto dto, string? emailConfirmRoute = null);
        Task ResetPasswordAsync(int userId, string token, string newPassword);
    }
}