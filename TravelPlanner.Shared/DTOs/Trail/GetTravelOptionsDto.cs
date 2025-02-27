namespace TravelPlanner.Shared.DTOs.Trail
{
    public record GetTravelOptionsDto
    {
        public int FromAttractionId { get; set; }
        public int ToAttractionId { get; set; }
        public List<TransportOptionsDto> TransportOptions { get; set; } = new();
    }
}
