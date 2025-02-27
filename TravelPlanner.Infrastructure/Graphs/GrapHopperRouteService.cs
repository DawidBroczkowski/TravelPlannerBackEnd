using System.Text.Json;
using TravelPlanner.Domain.Models.Graphs;

namespace TravelPlanner.Infrastructure.Graphs
{
    public class GrapHopperRouteService : IRouteService
    {
        private readonly HttpClient _httpClient;

        public GrapHopperRouteService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<RouteData> CalculateRoute
            (double startLat, double startLon, double endLat, double endLon, string mode = "car")
        {
            var url = $"http://localhost:8989/route" +
                $"?point={startLat.ToString(System.Globalization.CultureInfo.InvariantCulture)}," +
                $"{startLon.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
                $"&point={endLat.ToString(System.Globalization.CultureInfo.InvariantCulture)}," +
                $"{endLon.ToString(System.Globalization.CultureInfo.InvariantCulture)}" +
                $"&profile={mode}" +
                $"&locale=en" +
                $"&points_encoded=false";
            var response = await _httpClient.GetStringAsync(url);
            var data = JsonSerializer.Deserialize<RouteResponse>(response);

            return new RouteData
            {
                Distance = data!.Paths[0].Distance,
                Time = data!.Paths[0].Time
            };
        }
    }
}
