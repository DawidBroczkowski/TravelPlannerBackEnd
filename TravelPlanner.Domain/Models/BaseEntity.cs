using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TravelPlanner.Domain.Models
{
    public abstract record BaseEntity
    {
        [Required]
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

        [AllowNull]
        public DateTime? UpdatedAt { get; set; }

        [Required]
        public bool IsDeleted { get; set; } = false;

        [Required]
        public bool IsPublic { get; set; } = false;
    }
}
