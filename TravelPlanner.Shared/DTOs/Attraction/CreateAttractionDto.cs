using System.ComponentModel.DataAnnotations;
using TravelPlanner.Shared.DTOs.Attraction.Time;

namespace TravelPlanner.Shared.DTOs.Attraction
{
    public record CreateAttractionDto
    {
        [Required]
        [MaxLength(256)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public int LocalityId { get; set; }
        [Required]
        public string Street { get; set; } = string.Empty;
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }
        [Required]
        public bool IsPublic { get; set; } = true;


        public List<CreateScheduleDto> Schedule { get; set; } = new List<CreateScheduleDto>();
        public List<CreateSeasonalAvailabilityDto> SeasonalAvailability { get; set; } = new List<CreateSeasonalAvailabilityDto>();
        public List<int> TagIds { get; set; } = new List<int>();
        public int CategoryId { get; set; }
    }
}
