using System.ComponentModel.DataAnnotations;

namespace TravelPlanner.Shared.DTOs.Attraction
{
    public record CreateAttractionTagDto
    {
        [Required]
        [MaxLength(128)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MaxLength(512)]
        public string Description { get; set; } = string.Empty;
    }
}
