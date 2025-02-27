using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using TravelPlanner.Domain.Models.Attractions.Location;
using TravelPlanner.Domain.Models.Attractions.Time;
using TravelPlanner.Domain.Models.Trails;

namespace TravelPlanner.Domain.Models.Attractions
{
    public record Attraction : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        // <-- Attraction description -->

        [Required]
        [MaxLength(256)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [AllowNull]
        public virtual Guid? MainImageId { get; set; }
        [AllowNull]
        public virtual AttractionCategory? Category { get; set; }
        public virtual List<AttractionTag> Tags { get; set; } = new();
        public virtual List<Region> Regions { get; set; } = new();

        // <-- Attraction location -->

        [AllowNull]
        public virtual Address? Address { get; set; }

        // <-- Attraction contact information -->

        [AllowNull]
        public string? WebsiteUrl { get; set; }
        [AllowNull]
        public string? PhoneNumber { get; set; }
        [AllowNull]
        public string? Email { get; set; }

        // <-- Additional information -->
        public double EnergyLevel { get; set; }
        public double Popularity { get; set; }
        public TimeSpan AverageVisitDuration { get; set; }

        // <-- Attraction schedule -->
        public virtual List<Schedule> Schedules { get; set; } = new();
        public virtual List<SeasonalAvailability> SeasonalAvailabilities { get; set; } = new();
        public virtual List<SpecialDay> SpecialDays { get; set; } = new();


        public virtual List<Trail> Trails { get; set; } = new();
    }
}
