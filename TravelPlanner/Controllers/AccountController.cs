using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TravelPlanner.Application.Services.Interfaces;
using TravelPlanner.Services.Interfaces;
using TravelPlanner.Shared.DTOs.Auth;
using TravelPlanner.Shared.DTOs.User;

namespace TravelPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAccountService _accountService;
        private IUserProfileService _userProfileService;

        public AccountController(IAccountService accountService, IUserProfileService userProfileService)
        {
            _accountService = accountService;
            _userProfileService = userProfileService;
            Console.WriteLine("AccountController instantiated");
        }

        [HttpPost("register")]
        public async Task<ActionResult<string?>> RegisterUserAsync(RegisterUserDto dto)
        {
            var route = $"{Request.Scheme}://{Request.Host}/api/user/confirm-email"; // TODO: Change this to frontend route
            var token = await _accountService.RegisterUserAsync(dto, route);
            if (token is not null)
            {
                return Ok(token);
            }
            return Ok();
        }

        [HttpPost("confirm-email/{email}/{token}")]
        public async Task<ActionResult> ConfirmEmailAsync(string email, string token)
        {
            await _accountService.ConfirmEmailAsync(email, token);
            await _userProfileService.CreateProfileAsync(email, CancellationToken.None);
            return Ok();
        }

        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPasswordAsync([Required] string email)
        {
            var route = $"{Request.Scheme}://{Request.Host}/api/user/reset-password"; // TODO: Change this to frontend route
            await _accountService.ForgotPasswordAsync(email, route);
            return Ok();
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPasswordAsync(int userId, string token, string newPassword)
        {
            await _accountService.ResetPasswordAsync(userId, token, newPassword);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationDto>> LoginUserAsync([FromBody] LoginUserDto dto, CancellationToken cancellationToken)
        {
            var token = await _accountService.LoginUserAsync(dto, cancellationToken);

            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(token);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<AuthenticationDto>> RefreshTokenAsync(RefreshTokenDto dto, CancellationToken cancellationToken)
        {
            var token = await _accountService.RefreshTokenAsync(dto, cancellationToken);

            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(token);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> LogoutUserAsync(CancellationToken cancellationToken)
        {
            await _accountService.LogoutUserAsync(cancellationToken);
            return Ok();
        }
    }
}
