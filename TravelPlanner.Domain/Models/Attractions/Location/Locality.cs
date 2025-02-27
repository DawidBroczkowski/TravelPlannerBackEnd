using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelPlanner.Domain.Models.Attractions.Location
{
    public record Locality
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [ForeignKey(nameof(Province))]
        public int ProvinceId { get; set; }
        [Required]
        public virtual Province? Province { get; set; }
        public virtual List<Attraction> Attractions { get; set; } = new();
        public virtual List<ZipCode> ZipCodes { get; set; } = new();
    }
}
