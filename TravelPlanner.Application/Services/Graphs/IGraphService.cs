using TravelPlanner.Domain.Models.Graphs;
using TravelPlanner.Shared.DTOs.Trail;

namespace TravelPlanner.Application.Services.Graphs
{
    public interface IGraphService
    {
        Task<GetTravelOptionsDto> GetTravelOptions(int fromAttractionId, int toAttractionId, List<string> modes);
        Task PrecomputeGraphAsync();
        Task<TravelGraph> LoadGraphAsync();
        Task<TravelGraph> GetGraphAsync();
    }
}