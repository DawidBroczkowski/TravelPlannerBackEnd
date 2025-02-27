using Microsoft.EntityFrameworkCore;
using TravelPlanner.Shared.DTOs.Attraction;
using TravelPlanner.Shared.DTOs.Attraction.Location;
using TravelPlanner.Shared.DTOs.Attraction.Time;
using TravelPlanner.Shared.DTOs.File;

namespace TravelPlanner.Infrastructure.Views
{
    [Keyless]
    public class AttractionDetailsView
    {
        public int AttractionId { get; set; }
        public string AttractionName { get; set; } = string.Empty;
        public string AttractionDescription { get; set; } = string.Empty;
        public string? WebsiteUrl { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }

        public GetAddressDto? Address { get; set; }
        public GetAttractionCategoryDto? Category { get; set; }

        public List<GetAttractionTagDto> Tags { get; set; } = new();
        public List<GetRegionDto> Regions { get; set; } = new();
        public List<FileDataDto> Files { get; set; } = new();
        public FileDataDto? MainImage { get; set; }

        public List<GetScheduleDto> Schedules { get; set; } = new();
        public List<CreateSeasonalAvailabilityDto> SeasonalAvailabilities { get; set; } = new();
        public List<GetSpecialDayDto> SpecialDays { get; set; } = new();
    }

}
