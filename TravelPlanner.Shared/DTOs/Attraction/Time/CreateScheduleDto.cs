namespace TravelPlanner.Shared.DTOs.Attraction.Time
{
    public record CreateScheduleDto
    {
        public DayOfWeek DayOfWeek { get; set; }

        // Time slots for the day
        public virtual List<CreateScheduleTimeSlotDto> TimeSlots { get; set; } = new();
    }
}
