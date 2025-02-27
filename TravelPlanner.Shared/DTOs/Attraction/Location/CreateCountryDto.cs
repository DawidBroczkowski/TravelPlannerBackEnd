using System.ComponentModel.DataAnnotations;

namespace TravelPlanner.Shared.DTOs.Attraction.Location
{
    public record CreateCountryDto
    {
        [Required]
        [MaxLength(128)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MaxLength(512)]
        public string Description { get; set; } = string.Empty;
    }
}
