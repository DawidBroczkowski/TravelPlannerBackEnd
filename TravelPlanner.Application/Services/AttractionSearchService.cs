using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TravelPlanner.Application.Services.Interfaces;
using TravelPlanner.Infrastructure.Repositories;
using TravelPlanner.Shared.DTOs.Attraction;

namespace TravelPlanner.Application.Services
{
    public class AttractionSearchService : BaseService, IAttractionSearchService
    {
        private ISearchEngine _searchEngine;
        private ITrailRepository _trailRepository;
        private ITrailService _trailService;

        public AttractionSearchService(IServiceProvider serviceProvider, IConfiguration configuration) : base(serviceProvider, configuration)
        {
            _trailRepository = serviceProvider.GetRequiredService<ITrailRepository>();
            _trailService = serviceProvider.GetRequiredService<ITrailService>();
            _searchEngine = serviceProvider.GetRequiredService<ISearchEngine>();
        }

        public async Task<List<GetAttractionRecommendationDto>> GetSequentialAttractionRecommendationsAsync(int trailId,
            List<string> modesOfTransportation,
            string travelTimePreference,
            string visitTimePreference,
            string popularityPreference,
            string energyExpenditurePreference,
            int top,
            bool permissionOverride,
            CancellationToken cancellationToken)
        {
            var trail = await _trailService.GetTrailAsync(trailId, permissionOverride, cancellationToken);
            var visitedAttractions = await _trailRepository.GetAttractionsInTrailAsync(trailId, permissionOverride, cancellationToken);
            return await _searchEngine.FindNextAttractionAsync(visitedAttractions, modesOfTransportation, travelTimePreference, visitTimePreference, popularityPreference, energyExpenditurePreference, top);
        }
    }
}
