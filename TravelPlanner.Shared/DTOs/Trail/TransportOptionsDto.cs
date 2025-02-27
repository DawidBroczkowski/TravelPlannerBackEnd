namespace TravelPlanner.Shared.DTOs.Trail
{
    public record TransportOptionsDto
    {
        public string Mode { get; set; } = string.Empty;
        public double Time { get; set; }
        public double Distance { get; set; }
    }
}
