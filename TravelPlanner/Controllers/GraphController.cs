using Microsoft.AspNetCore.Mvc;
using TravelPlanner.Application.Services.Graphs;
using TravelPlanner.Shared.DTOs.Trail;

namespace TravelPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GraphController : ControllerBase
    {
        private IGraphService _graphService;

        public GraphController(IGraphService graphService)
        {
            _graphService = graphService;
        }

        [HttpPost]
        public async Task<IActionResult> PrecomputeGraph()
        {
            await _graphService.PrecomputeGraphAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult> LoadGraph()
        {
            var graph = await _graphService.LoadGraphAsync();
            return Ok(graph);
        }

        [HttpPost("travel-options")]
        public async Task<ActionResult<GetTravelOptionsDto>> GetTravelOptions(PostTravelOptionsDto dto)
        {
            var travelOptions = await _graphService.GetTravelOptions(dto.FromAttractionId, dto.ToAttractionId, dto.Modes);
            return Ok(travelOptions);
        }
    }
}
