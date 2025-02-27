using Microsoft.AspNetCore.Mvc;
using TravelPlanner.Application.Services.Interfaces;
using TravelPlanner.Domain.Models;
using TravelPlanner.Misc;
using TravelPlanner.Shared.DTOs.Penalty;

namespace TravelPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModerationController : ControllerBase
    {
        private readonly IModerationService _moderationService;

        public ModerationController(IModerationService moderationService)
        {
            _moderationService = moderationService;
        }

        [HttpPost("penalty")]
        [RequirePermission(Permission.ModerateUsers)]
        public async Task<IActionResult> ApplyPenaltyToUserAsync([FromBody] CreatePenaltyDto dto)
        {
            await _moderationService.ApplyPenaltyToUserAsync(dto, CancellationToken.None);
            return Ok();
        }
    }
}
