using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TravelPlanner.Domain.Models.Attractions.Time
{
    public record SpecialDay
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public bool IsOpen { get; set; } // True for open, False for closed
        [AllowNull]
        public TimeSpan? OpeningTime { get; set; } // Optional specific hours for that day
        [AllowNull]
        public TimeSpan? ClosingTime { get; set; } // Optional specific hours for that day

        // Relation to Attraction
        [Required]
        public virtual Attraction Attraction { get; set; } = new();
    }
}
