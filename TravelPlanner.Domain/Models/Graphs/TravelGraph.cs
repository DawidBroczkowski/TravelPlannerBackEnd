namespace TravelPlanner.Domain.Models.Graphs
{
    public record TravelGraph
    {
        public Dictionary<string, Dictionary<int, Dictionary<int, RouteData>>> ModeRoutes { get; set; } = new();
    }
}
