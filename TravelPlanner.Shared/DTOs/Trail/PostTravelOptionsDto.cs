namespace TravelPlanner.Shared.DTOs.Trail
{
    public record PostTravelOptionsDto
    {
        public int FromAttractionId { get; init; }
        public int ToAttractionId { get; init; }
        public List<string> Modes { get; init; } = new List<string>();
    }
}
