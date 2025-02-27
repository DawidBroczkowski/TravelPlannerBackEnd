using System.ComponentModel.DataAnnotations;

namespace TravelPlanner.Domain.Models.Attractions.Time
{
    public record SeasonalAvailability
    {
        [Key]
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsOpen { get; set; } // True for open, False for closed

        // Relation to Attraction
        [Required]
        public virtual Attraction Attraction { get; set; } = new();
    }

}
