using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelPlanner.Domain.Models.Attractions.Location
{
    public record Province
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [ForeignKey(nameof(Country))]
        public int CountryId { get; set; }
        [Required]
        public virtual Country? Country { get; set; }
        [Required]
        public virtual List<Locality>? Localities { get; init; }
    }
}
