namespace TravelPlanner.Shared.DTOs.Attraction.Time
{
    public record CreateScheduleTimeSlotDto
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
