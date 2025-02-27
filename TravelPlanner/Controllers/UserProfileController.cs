using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelPlanner.Application.Services.Interfaces;
using TravelPlanner.Domain.Models;
using TravelPlanner.Shared.DTOs.UserProfile;

namespace TravelPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;

        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<GetUserProfileDto>> GetUserProfileAsync(int userId, CancellationToken cancellationToken)
        {
            Enum.TryParse(User.Claims.FirstOrDefault(c => c.Type == "permissions")?.Value, out Permission permissions);
            bool hasPrivateContentAccess = (permissions & Permission.AccessPrivateContent) == Permission.AccessPrivateContent;

            var profile = await _userProfileService.GetUserProfileAsync(userId, hasPrivateContentAccess, cancellationToken);

            return Ok(profile);
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult> UpdateUserProfileAsync(UpdateUserProfileDto dto, CancellationToken cancellationToken)
        {
            Enum.TryParse(User.Claims.FirstOrDefault(c => c.Type == "permissions")?.Value, out Permission permissions);
            bool hasManagementPermission = (permissions & Permission.ManageUsers) == Permission.ManageUsers;

            await _userProfileService.UpdateUserProfileAsync(dto, hasManagementPermission, cancellationToken);

            return Ok();
        }

        [Authorize]
        [HttpPatch]
        public async Task<ActionResult> AssignProfileImageAsync([FromQuery] Guid fileId, CancellationToken cancellationToken)
        {
            await _userProfileService.AssignProfileImageAsync(fileId, cancellationToken);

            return Ok();
        }
    }
}
