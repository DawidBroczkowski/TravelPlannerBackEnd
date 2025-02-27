using System.ComponentModel.DataAnnotations;

namespace TravelPlanner.Domain.Models.Attractions.Time
{
    public record Schedule
    {
        [Key]
        public int Id { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        // Time slots for the day
        public virtual List<ScheduleTimeSlot> TimeSlots { get; set; } = new();

        // Relation to Attraction
        [Required]
        public virtual Attraction Attraction { get; set; } = new();
    }
}
