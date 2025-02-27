namespace TravelPlanner.Shared.DTOs.Trail
{
    public record CreateTrailDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<int> AttractionIds { get; set; } = new List<int>();
        public int CreatedById { get; set; } = 0;
    }
}
