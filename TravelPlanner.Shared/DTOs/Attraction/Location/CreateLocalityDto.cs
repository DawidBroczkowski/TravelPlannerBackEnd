using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TravelPlanner.Shared.DTOs.Attraction.Location
{
    public record CreateLocalityDto
    {
        [Required]
        [MaxLength(128)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [MaxLength(512)]
        public string Description { get; set; } = string.Empty;
        [AllowNull]
        public int? ProvinceId { get; set; }
    }
}
