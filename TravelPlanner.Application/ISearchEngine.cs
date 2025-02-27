using TravelPlanner.Shared.DTOs.Attraction;
using TravelPlanner.Shared.DTOs.Trail;

namespace TravelPlanner.Application
{
    public interface ISearchEngine
    {
        Task<List<GetAttractionRecommendationDto>> FindNextAttractionAsync(List<GetAttractionInTrailDto> attractions, List<string> modesOfTransportation, string travelTimePreference, string visitTimePreference, string popularityPreference, string energyExpenditurePreference, int top);
    }
}