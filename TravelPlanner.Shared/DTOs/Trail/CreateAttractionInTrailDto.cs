namespace TravelPlanner.Shared.DTOs.Trail
{
    public record CreateAttractionInTrailDto
    {
        public int AttractionId { get; set; }
        public int TrailId { get; set; }
        public string TransportationMode { get; set; } = string.Empty;
        public double TravelTime { get; set; }
        public double TravelDistance { get; set; }
        public int Order { get; set; }
    }
}
