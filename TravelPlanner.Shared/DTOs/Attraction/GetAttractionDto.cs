using TravelPlanner.Shared.DTOs.Attraction.Location;
using TravelPlanner.Shared.DTOs.Attraction.Time;

namespace TravelPlanner.Shared.DTOs.Attraction
{
    public record GetAttractionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? WebsiteUrl { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsPublic { get; set; }
        public double EnergyLevel { get; set; }
        public double Popularity { get; set; }
        public TimeSpan AverageVisitDuration { get; set; }

        public GetAddressDto? Address { get; set; }
        public GetAttractionCategoryDto? Category { get; set; }
        public List<GetAttractionTagDto> Tags { get; set; } = new();
        public List<GetRegionDto> Regions { get; set; } = new();
        public List<Guid> FileIds { get; set; } = new();
        public Guid? MainImageId { get; set; }
        public List<GetScheduleDto> Schedules { get; set; } = new();
        public List<GetSeasonalAvailabilityDto> SeasonalAvailabilities { get; set; } = new();
        public List<GetSpecialDayDto> SpecialDays { get; set; } = new();
    }
}
