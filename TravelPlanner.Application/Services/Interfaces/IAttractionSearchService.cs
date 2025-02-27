using TravelPlanner.Shared.DTOs.Attraction;

namespace TravelPlanner.Application.Services.Interfaces
{
    public interface IAttractionSearchService
    {
        Task<List<GetAttractionRecommendationDto>> GetSequentialAttractionRecommendationsAsync(int trailId, List<string> modesOfTransportation, string travelTimePreference, string visitTimePreference, string popularityPreference, string energyExpenditurePreference, int top, bool permissionOverride, CancellationToken cancellationToken);
    }
}