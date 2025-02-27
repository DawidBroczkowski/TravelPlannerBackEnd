using System.ComponentModel.DataAnnotations;
using TravelPlanner.Domain.Models.Attractions;

namespace TravelPlanner.Domain.Models.Trails
{
    public record AttractionInTrail
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public virtual Attraction? Attraction { get; set; }
        [Required]
        public virtual Trail? Trail { get; set; }
        public string TransportationMode { get; set; } = string.Empty;
        public double TravelTime { get; set; }
        public double TravelDistance { get; set; }
        public int Order { get; set; }
    }
}
