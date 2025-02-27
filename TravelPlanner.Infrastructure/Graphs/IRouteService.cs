using TravelPlanner.Domain.Models.Graphs;

namespace TravelPlanner.Infrastructure.Graphs
{
    public interface IRouteService
    {
        Task<RouteData> CalculateRoute(double startLat, double startLon, double endLat, double endLon, string mode = "driving");
    }
}