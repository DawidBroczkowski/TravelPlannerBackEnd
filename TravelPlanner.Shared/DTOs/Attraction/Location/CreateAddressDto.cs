using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TravelPlanner.Shared.DTOs.Attraction.Location
{
    public record CreateAddressDto
    {
        [Required]
        [MaxLength(128)]
        public string Street { get; set; } = string.Empty;
        [Required]
        [MaxLength(128)]
        public string City { get; set; } = string.Empty;
        [AllowNull]
        public int? LocalityId { get; set; }
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }
    }
}
