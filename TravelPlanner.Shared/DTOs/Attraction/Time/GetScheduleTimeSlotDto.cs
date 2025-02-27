namespace TravelPlanner.Shared.DTOs.Attraction.Time
{
    public record GetScheduleTimeSlotDto
    {
        public int Id { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
