using Microsoft.Extensions.Configuration;
using TravelPlanner.Domain.Models.Graphs;
using TravelPlanner.Infrastructure.Graphs;
using TravelPlanner.Shared.DTOs.Attraction;

namespace TravelPlanner.Application.Services.Graphs
{
    public class GraphBuilder
    {
        private readonly IRouteService _precomputer;
        private readonly List<GetAttractionDto> _attractions;
        private readonly List<string> _modes = new();

        public GraphBuilder(IRouteService precomputer, IConfiguration configuration, List<GetAttractionDto> attractions)
        {
            _precomputer = precomputer;
            _attractions = attractions;
            _modes = configuration.GetSection("TransportationModes").Get<List<string>>()!;
        }

        public async Task<TravelGraph> BuildGraph()
        {
            var graph = new TravelGraph();

            foreach (var mode in _modes)
            {
                graph.ModeRoutes[mode] = new Dictionary<int, Dictionary<int, RouteData>>();

                foreach (var start in _attractions)
                {
                    graph.ModeRoutes[mode][start.Id] = new Dictionary<int, RouteData>();

                    foreach (var end in _attractions.Where(a => a.Id != start.Id))
                    {
                        var route = await _precomputer.CalculateRoute(
                            start.Address!.Latitude, start.Address.Longitude,
                            end.Address!.Latitude, end.Address!.Longitude,
                            mode
                        );

                        graph.ModeRoutes[mode][start.Id][end.Id] = route;
                    }
                }
            }

            return graph;
        }
    }
}
