using TravelPlanner.Shared.DTOs.Attraction;

namespace TravelPlanner.Shared.DTOs.Trail
{
    public record GetAttractionInTrailDto
    {
        public int Id { get; set; }
        public int TrailId { get; set; }
        public int Order { get; set; }
        public GetAttractionDto? Attraction { get; set; }
        public string TransportationMode { get; set; } = string.Empty;
        public double TravelTime { get; set; }
        public double TravelDistance { get; set; }
        public bool IsPublic { get; set; }
    }
}
