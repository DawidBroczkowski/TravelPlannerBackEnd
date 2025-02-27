using System.ComponentModel.DataAnnotations;

namespace TravelPlanner.Shared.DTOs.Attraction
{
    public record CreateAttractionCategoryDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
    }
}
