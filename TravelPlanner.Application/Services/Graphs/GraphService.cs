using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using TravelPlanner.Domain.Models.Graphs;
using TravelPlanner.Infrastructure.Graphs;
using TravelPlanner.Infrastructure.Repositories.Interfaces;
using TravelPlanner.Shared.DTOs.Trail;

namespace TravelPlanner.Application.Services.Graphs
{
    public class GraphService : BaseService, IGraphService
    {
        private IAttractionRepository _attractionRepository;
        private IRouteService _routeService;
        private IGraphRepository _graphRepository;
        private TravelGraphAccessor _graphAccessor;
        private TravelGraph _emptyGraph = new TravelGraph();

        public GraphService(IServiceProvider serviceProvider, IConfiguration configuration) : base(serviceProvider, configuration)
        {
            _attractionRepository = serviceProvider.GetRequiredService<IAttractionRepository>();
            _routeService = serviceProvider.GetRequiredService<IRouteService>();
            _graphRepository = serviceProvider.GetRequiredService<IGraphRepository>();
            _graphAccessor = serviceProvider.GetRequiredService<TravelGraphAccessor>();
        }

        public async Task<GetTravelOptionsDto> GetTravelOptions(int fromAttractionId, int toAttractionId, List<string> modes)
        {
            var graph = await GetGraphAsync();

            GetTravelOptionsDto travelOptions = new GetTravelOptionsDto
            {
                FromAttractionId = fromAttractionId,
                ToAttractionId = toAttractionId
            };

            foreach (var mode in modes)
            {
                var path = graph.ModeRoutes[mode][fromAttractionId][toAttractionId];
                TransportOptionsDto transportOption = new TransportOptionsDto
                {
                    Mode = mode,
                    Time = path.Time,
                    Distance = path.Distance
                };
                travelOptions.TransportOptions.Add(transportOption);
            }

            return travelOptions;
        }

        public async Task PrecomputeGraphAsync()
        {
            var attractions = await _attractionRepository
                .GetAttractionsAsync(1, int.MaxValue, false, cancellationToken: default);
            var builder = new GraphBuilder(_routeService, _configuration, attractions);
            var graph = await builder.BuildGraph();
            var json = JsonSerializer.Serialize(graph);
            _graphAccessor.Graph = graph;
            await _graphRepository.SaveGraphAsync(json);
        }

        public async Task<TravelGraph> LoadGraphAsync()
        {
            var json = await _graphRepository.LoadGraphAsync();
            _graphAccessor.Graph = JsonSerializer.Deserialize<TravelGraph>(json)!;
            return _graphAccessor.Graph;
        }

        public async Task<TravelGraph> GetGraphAsync()
        {
            if (_graphAccessor.Graph is null)
            {
                _graphAccessor.Graph = await LoadGraphAsync();
            }

            return _graphAccessor.Graph;
        }
    }
}
