namespace TravelPlanner.Shared.DTOs.Attraction.Time
{
    public record GetSpecialDayDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan? OpeningTime { get; set; }
        public TimeSpan? ClosingTime { get; set; }
        public bool IsOpen { get; set; }
    }
}
