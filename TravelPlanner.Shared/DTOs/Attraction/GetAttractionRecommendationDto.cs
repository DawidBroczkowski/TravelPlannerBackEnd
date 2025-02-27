namespace TravelPlanner.Shared.DTOs.Attraction
{
    public record GetAttractionRecommendationDto
    {
        public GetAttractionDto Attraction { get; set; } = new GetAttractionDto();
        public string TransportationMode { get; set; } = string.Empty;
        public double TravelDistance { get; set; }
        public double TravelTime { get; set; }
        public double Score { get; set; }
    }
}
