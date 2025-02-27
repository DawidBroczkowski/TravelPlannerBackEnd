using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TravelPlanner.Domain.Models.Attractions.Location
{
    public record Address : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        [AllowNull]
        public virtual Locality? Locality { get; set; }
        [AllowNull]
        public virtual ZipCode? ZipCode { get; set; }
        [AllowNull]
        public string? Street { get; set; } = string.Empty;
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }
    }
}
