using System.ComponentModel.DataAnnotations;

namespace TravelPlanner.Domain.Models.Attractions.Location
{
    public record Region
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(256)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        public virtual List<Attraction> Attractions { get; set; } = new();
    }
}
