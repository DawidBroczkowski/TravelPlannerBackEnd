using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelPlanner.Application.Services.Interfaces;
using TravelPlanner.Domain.Models;
using TravelPlanner.Misc;
using TravelPlanner.Shared.DTOs.Attraction;
using TravelPlanner.Shared.DTOs.Trail;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TravelPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrailController : ControllerBase
    {
        private ITrailService _trailService;
        private IAttractionSearchService _searchService;

        public TrailController(IServiceProvider serviceProvider)
        {
            _trailService = serviceProvider.GetRequiredService<ITrailService>();
            try
            {
                _searchService = serviceProvider.GetRequiredService<IAttractionSearchService>();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("TrailController instantiated");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetTrailDto>> GetTrailAsync(int id, CancellationToken cancellationToken)
        {
            Enum.TryParse(User.Claims.FirstOrDefault(c => c.Type == "permissions")?.Value, out Permission permissions);
            bool hasPrivateContentAccess = (permissions & Permission.AccessPrivateContent) == Permission.AccessPrivateContent;

            var trail = await _trailService.GetTrailAsync(id, hasPrivateContentAccess, cancellationToken);
            return Ok(trail);
        }

        [HttpGet]
        public async Task<ActionResult<List<GetTrailDto>>> GetTrailsAsync(
            [FromQuery] int from,
            [FromQuery] int to,
            [FromQuery] bool onlyPublic,
            CancellationToken cancellationToken,
            [FromQuery] int? countryId = null,
            [FromQuery] int? provinceId = null,
            [FromQuery] int? localityId = null)
        {
            var trails = await _trailService.GetTrailsAsync(from, to, onlyPublic, cancellationToken, countryId, provinceId, localityId);
            return Ok(trails);
        }

        [Authorize]
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<GetTrailDto>>> GetUserTrailsAsync(int userId, [FromQuery] int from, [FromQuery] int to, CancellationToken cancellationToken)
        {
            Enum.TryParse(User.Claims.FirstOrDefault(c => c.Type == "permissions")?.Value, out Permission permissions);
            bool hasPrivateContentAccess = (permissions & Permission.AccessPrivateContent) == Permission.AccessPrivateContent;

            var trails = await _trailService.GetUserTrailsAsync(userId, from, to, hasPrivateContentAccess, cancellationToken);
            return Ok(trails);
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateTrailAsync(CreateTrailDto trailDto, CancellationToken cancellationToken)
        {
            int id = await _trailService.CreateTrailAsync(trailDto, cancellationToken);
            return Ok(id);
        }

        [HttpGet("{trailId}/{attractions}/recommend")]
        public async Task<ActionResult<List<GetAttractionRecommendationDto>>> GetSequentialAttractionRecommendations(int trailId,
            [FromQuery] List<string> modesOfTransportation,
            [FromQuery] string travelTimePreference,
            [FromQuery] string visitTimePreference,
            [FromQuery] string popularityPreference,
            [FromQuery] string energyExpenditurePreference,
            [FromQuery] int top,
            CancellationToken cancellationToken)
        {
            Enum.TryParse(User.Claims.FirstOrDefault(c => c.Type == "permissions")?.Value, out Permission permissions);
            bool hasPrivateContentAccess = (permissions & Permission.AccessPrivateContent) == Permission.AccessPrivateContent;

            var recommendations = await _searchService.GetSequentialAttractionRecommendationsAsync(trailId, modesOfTransportation, travelTimePreference, visitTimePreference, popularityPreference, energyExpenditurePreference, top, hasPrivateContentAccess, cancellationToken);
            return Ok(recommendations);
        }

        [HttpGet("attractions/{attractionInTrailId}")]
        public async Task<ActionResult<GetAttractionInTrailDto>> GetAttractionInTrailAsync(int trailId, int attractionInTrailId, CancellationToken cancellationToken)
        {
            Enum.TryParse(User.Claims.FirstOrDefault(c => c.Type == "permissions")?.Value, out Permission permissions);
            bool hasPrivateContentAccess = (permissions & Permission.AccessPrivateContent) == Permission.AccessPrivateContent;

            var attraction = await _trailService.GetAttractionInTrailAsync(attractionInTrailId, hasPrivateContentAccess, cancellationToken);
            return Ok(attraction);
        }

        [HttpGet("{trailId}/attractions")]
        public async Task<ActionResult<List<GetAttractionInTrailDto>>> GetAttractionsInTrailAsync(int trailId, CancellationToken cancellationToken)
        {
            Enum.TryParse(User.Claims.FirstOrDefault(c => c.Type == "permissions")?.Value, out Permission permissions);
            bool hasPrivateContentAccess = (permissions & Permission.AccessPrivateContent) == Permission.AccessPrivateContent;

            var attractions = await _trailService.GetAttractionsInTrailAsync(trailId, hasPrivateContentAccess, hasPrivateContentAccess, cancellationToken);
            return Ok(attractions);
        }

        // needs permission overrides
        [Authorize]
        [RequirePermission(Permission.CreateTravelPlan)]
        [HttpPost("{trailId}/attractions")]
        public async Task<ActionResult<int>> AddAttractionToTrailAsync(int trailId, CreateAttractionInTrailDto dto, CancellationToken cancellationToken)
        {
            int id = await _trailService.AddAttractionToTrailAsync(dto, cancellationToken);
            return Ok(id);
        }

        [Authorize]
        [RequirePermission(Permission.CreateTravelPlan)]
        [HttpDelete("{trailId}/attractions/{attractionInTrailId}")]
        public async Task<ActionResult> RemoveAttractionFromTrailAsync(int trailId, int attractionInTrailId, CancellationToken cancellationToken)
        {
            await _trailService.RemoveAttractionFromTrailAsync(new CreateAttractionInTrailDto { TrailId = trailId, AttractionId = attractionInTrailId }, cancellationToken);
            return Ok();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTrailAsync(int id, CancellationToken cancellationToken)
        {
            await _trailService.DeleteTrailAsync(id, cancellationToken);
            return Ok();
        }
    }
}
