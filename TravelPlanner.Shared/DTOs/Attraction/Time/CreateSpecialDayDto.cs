using System.Diagnostics.CodeAnalysis;

namespace TravelPlanner.Shared.DTOs.Attraction.Time
{
    public record CreateSpecialDayDto
    {
        public DateTime Date { get; set; }
        public bool IsOpen { get; set; } // True for open, False for closed
        [AllowNull]
        public TimeSpan? OpeningTime { get; set; } // Optional specific hours for that day
        [AllowNull]
        public TimeSpan? ClosingTime { get; set; }
    }
}
