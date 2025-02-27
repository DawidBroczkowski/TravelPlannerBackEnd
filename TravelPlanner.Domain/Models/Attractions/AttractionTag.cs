using System.ComponentModel.DataAnnotations;

namespace TravelPlanner.Domain.Models.Attractions
{
    public record AttractionTag
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        public virtual List<Attraction> Attractions { get; set; } = new();
    }
}
