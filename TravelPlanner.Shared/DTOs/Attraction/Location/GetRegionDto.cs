namespace TravelPlanner.Shared.DTOs.Attraction.Location
{
    public record GetRegionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
