namespace TravelPlanner.Shared.DTOs.Attraction.Time
{
    public record GetScheduleDto
    {
        public int Id { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public List<GetScheduleTimeSlotDto> TimeSlots { get; set; } = new();
    }
}
