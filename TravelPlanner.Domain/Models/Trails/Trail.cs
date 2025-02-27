using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TravelPlanner.Domain.Models.Trails
{
    public record Trail : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(128)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(512)]
        public string Description { get; set; } = string.Empty;
        [AllowNull]
        public Guid? ImageId { get; set; }
        public TimeSpan Duration { get; set; }
        public virtual List<AttractionInTrail>? Attractions { get; set; }
        public virtual ApplicationUser? CreatedBy { get; set; }
    }
}
