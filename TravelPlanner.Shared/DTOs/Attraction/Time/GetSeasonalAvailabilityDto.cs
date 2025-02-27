namespace TravelPlanner.Shared.DTOs.Attraction.Time
{
    public record GetSeasonalAvailabilityDto
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsOpen { get; set; }
    }
}
