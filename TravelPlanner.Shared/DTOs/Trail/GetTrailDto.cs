namespace TravelPlanner.Shared.DTOs.Trail
{
    public record GetTrailDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TimeSpan Duration { get; set; }
        public List<int> AttractionInTrailIds { get; set; } = new List<int>();
        public int CreatedById { get; set; } = 0;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsPublic { get; set; }

        public List<string> TransportationModes { get; set; } = new List<string>();
        public double TotalTravelDistance { get; set; }
        public TimeSpan TotalTravelTime { get; set; }
    }
}
