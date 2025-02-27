using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelPlanner.Application.Services.Interfaces;
using TravelPlanner.Domain.Models;
using TravelPlanner.Infrastructure.Repositories.Interfaces;
using TravelPlanner.Misc;
using TravelPlanner.Shared.DTOs.Penalty;
using TravelPlanner.Shared.DTOs.User;

namespace TravelPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        private IJwtBlacklistRepository _repository;

        public UserController(IUserService userService, IJwtBlacklistRepository repository)
        {
            _userService = userService;
            _repository = repository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetUserDto>> GetUserAsync(int id, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserAsync(id, cancellationToken);

            if (user is null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [Authorize]
        [RequirePermission(Permission.CommentOnAttraction)]
        [HttpGet("{id}/penalties")]
        public async Task<ActionResult<List<GetPenaltyDto>>> GetUserPenaltiesAsync(int id, CancellationToken cancellationToken)
        {
            var penalties = await _userService.GetUserPenaltiesAsync(id, cancellationToken);

            if (penalties is null)
            {
                return NotFound();
            }

            return Ok(penalties);
        }
    }
}
