using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using System.ComponentModel.DataAnnotations;
using TravelPlanner.Application;
using TravelPlanner.Domain.Models;
using TravelPlanner.Infrastructure.Email.Interfaces;
using TravelPlanner.Infrastructure.Repositories.Interfaces;
using TravelPlanner.Services.Interfaces;
using TravelPlanner.Shared.DTOs.Auth;
using TravelPlanner.Shared.DTOs.User;

namespace TravelPlanner.Services
{
    public class EmailAccountService : BaseService, IAccountService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAuthService _authService;
        private readonly IJwtBlacklistRepository _jwtBlacklistRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EmailAccountService(IServiceProvider serviceProvider, IConfiguration configuration) : base(serviceProvider, configuration)
        {
            _signInManager = _serviceProvider.GetRequiredService<SignInManager<ApplicationUser>>();
            _authService = _serviceProvider.GetRequiredService<IAuthService>();
            _jwtBlacklistRepository = _serviceProvider.GetRequiredService<IJwtBlacklistRepository>();
            _userRepository = _serviceProvider.GetRequiredService<IUserRepository>();
            _emailService = _serviceProvider.GetRequiredService<IEmailService>();
            _httpContextAccessor = _serviceProvider.GetRequiredService<IHttpContextAccessor>();
        }

        public async Task<string?> RegisterUserAsync(RegisterUserDto dto, string? emailConfirmRoute = null)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Name,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                // Handle registration errors
                var ex = new ValidationException("Registration failed");
                foreach (var error in result.Errors)
                {
                    ex.Data.Add(error.Code, error.Description);
                }
                throw ex;
            }

            // Assign a default role
            await _userManager.AddToRoleAsync(user, "User");

            // Generate email confirmation token
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // Generate the URL using LinkGenerator
            if (emailConfirmRoute is null)
            {
                return null;
            }

            var confirmationLink = $"{emailConfirmRoute}?email={user.Email}&token={token}";

            // Send the confirmation email
            await _emailService.SendEmailAsync(user.Email, "Confirm your email",
                $"Please confirm your account by clicking here: <a href='{confirmationLink}'>Confirm Email</a>");

            return token;
        }

        public async Task ConfirmEmailAsync(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                var ex = new ValidationException("User with this email does not exist");
                ex.Data.Add(nameof(email), email);
                throw ex;
            }

            if (user.EmailConfirmed is true)
            {
                return;
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                var ex = new ValidationException("Email confirmation failed");
                foreach (var error in result.Errors)
                {
                    ex.Data.Add(error.Code, error.Description);
                }
                throw ex;
            }
        }

        public async Task ForgotPasswordAsync(string email, string passwordResetRoute)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Generate the URL using LinkGenerator
            var resetLink = $"{passwordResetRoute}?email={user.Email}&token={token}";

            // Send the reset email
            await _emailService.SendEmailAsync(user.Email!, "Reset your password",
                $"Please reset your password by clicking here: <a href='{resetLink}'>Reset Password</a>");
        }

        /// <summary>
        /// Resets a forgotten password.
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task ResetPasswordAsync(int userId, string token, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
            {
                var ex = new ValidationException("User with this Id does not exist");
                ex.Data.Add(nameof(userId), userId);
                throw ex;
            }

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (result.Succeeded is false)
            {
                var ex = new ValidationException("Password reset failed");
                foreach (var error in result.Errors)
                {
                    ex.Data.Add(error.Code, error.Description);
                }
                throw ex;
            }
        }

        public async Task ChangePasswordAsync(ChangePasswordDto dto)
        {
            var user = await _userManager.FindByIdAsync(_userId.ToString());  // Gets the currently logged-in user
            if (user == null)
            {
                var ex = new UnauthorizedAccessException("User is not logged in");
                throw ex;
            }

            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (result.Succeeded is false)
            {
                var ex = new ValidationException("Password change failed");
                foreach (var error in result.Errors)
                {
                    ex.Data.Add(error.Code, error.Description);
                }
                throw ex;
            }

            // Send an email notification
            await _emailService.SendEmailAsync(user.Email!, "Password changed",
                "Your password has been changed successfully");
        }

        public async Task<AuthenticationDto> LoginUserAsync(LoginUserDto dto, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null)
            {
                throw new ValidationException("Invalid email or password");
            }

            var result = await _signInManager.PasswordSignInAsync(user, dto.Password, false, false);
            if (!result.Succeeded)
            {
                throw new ValidationException("Invalid email or password");
            }

            // Handle JTI and token logic as before
            (string accessToken, Guid jti, DateTime jtiExpiry, long permissions) = await _authService.CreateAccessTokenAsync(user, cancellationToken);
            var refreshToken = _authService.CreateRefreshToken(out DateTime refreshTokenExpiry);

            await _userRepository.UpdateTokensAsync(user.Id, refreshToken, refreshTokenExpiry, jti, jtiExpiry, cancellationToken);
            //appendTokensToResponse(dto.RememberMe, accessToken, refreshToken, refreshTokenExpiry);

            return new AuthenticationDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiry = jtiExpiry,
                Permissions = permissions
            };
        }

        public async Task<AuthenticationDto> RefreshTokenAsync(bool rememberMe, CancellationToken cancellationToken)
        {
            var context = _httpContextAccessor.HttpContext!;
            var cookieRefreshToken = context.Request.Cookies["refreshToken"];
            if (cookieRefreshToken is null)
            {
                var ex = new UnauthorizedAccessException("No refresh token found");
                throw ex;
            }

            var userDto = await _userRepository.GetUserByRefreshTokenAsync(cookieRefreshToken!, cancellationToken);
            if (userDto!.IsBanned is true)
            {
                var ex = new UnauthorizedAccessException("User is banned");
                ex.Data.Add(nameof(_userId), _userId);
                throw ex;
            }

            var oldRefreshToken = await _userRepository.GetUserRefreshTokenAsync(_userId, cancellationToken);
            if (oldRefreshToken != cookieRefreshToken)
            {
                var ex = new ValidationException("Invalid refresh token");
                throw ex;
            }

            if (userDto.RefreshTokenExpiry < DateTime.UtcNow)
            {
                var ex = new UnauthorizedAccessException("Refresh token expired");
                throw ex;
            }

            if (userDto.Jti is not null && userDto.JtiExpiry > DateTime.UtcNow)
            {
                await _jwtBlacklistRepository.BlacklistTokenAsync(userDto.Jti.ToString()!, userDto.JtiExpiry!.Value.AddMinutes(1));
            }

            var user = await _userManager.FindByIdAsync(userDto!.Id.ToString());

            (string accessToken, Guid jti, DateTime jtiExpiry, long permissions) = await _authService.CreateAccessTokenAsync(user!, cancellationToken);
            var newRefreshToken = _authService.CreateRefreshToken(out DateTime refreshTokenExpiry);

            await _userRepository.UpdateTokensAsync(user!.Id, newRefreshToken, refreshTokenExpiry, jti, jtiExpiry, cancellationToken);
            //appendTokensToResponse(rememberMe, accessToken, newRefreshToken, refreshTokenExpiry);

            return new AuthenticationDto
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken,
                AccessTokenExpiry = jtiExpiry,
                Permissions = permissions
            };
        }

        public async Task LogoutUserAsync(CancellationToken cancellationToken)
        {
            var jti = _userContext.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var jtiExpiryUnix = _userContext.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;
            var jtiExpiry = DateTimeOffset.FromUnixTimeSeconds(long.Parse(jtiExpiryUnix!)).UtcDateTime;

            if (jtiExpiry > DateTime.UtcNow)
            {
                await _jwtBlacklistRepository.BlacklistTokenAsync(jti!.ToString()!, jtiExpiry!.AddMinutes(1));
            }
        }

        private void appendTokensToResponse(bool rememberMe, string token, string refreshToken, DateTime refreshTokenExpiry)
        {
            var context = _httpContextAccessor.HttpContext!;

            context.Response.Cookies.Append("accessToken", token,
                new CookieOptions
                {
                    Expires = null,
                    HttpOnly = true,
                    IsEssential = true,
                    Secure = true,
                    SameSite = SameSiteMode.None
                });

            context.Response.Cookies.Append("refreshToken", refreshToken,
                new CookieOptions
                {
                    Expires = rememberMe ? refreshTokenExpiry : null,
                    HttpOnly = true,
                    IsEssential = true,
                    Secure = true,
                    SameSite = SameSiteMode.None
                });
        }

        public Task<AuthenticationDto> RefreshTokenAsync(RefreshTokenDto dto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
