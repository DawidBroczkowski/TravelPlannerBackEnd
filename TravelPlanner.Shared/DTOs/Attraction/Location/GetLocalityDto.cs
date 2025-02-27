using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TravelPlanner.Shared.DTOs.Attraction.Location
{
    public record GetLocalityDto
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [AllowNull]
        public int? ProvinceId { get; set; }
        [AllowNull]
        public GetProvinceDto? Province { get; set; }
    }
}
