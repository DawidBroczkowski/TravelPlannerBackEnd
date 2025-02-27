using System.ComponentModel.DataAnnotations;

namespace TravelPlanner.Domain.Models.Attractions.Time
{
    public record ScheduleTimeSlot
    {
        [Key]
        public int Id { get; set; }

        // Time range
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        // Relation to Schedule
        [Required]
        public virtual Schedule Schedule { get; set; } = new();
    }

}
