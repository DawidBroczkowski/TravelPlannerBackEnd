namespace TravelPlanner.Shared.DTOs.Attraction.Time
{
    public record CreateSeasonalAvailabilityDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsOpen { get; set; }
    }
}
