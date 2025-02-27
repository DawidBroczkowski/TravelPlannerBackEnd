using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TravelPlanner.Shared.DTOs.UserProfile
{
    public record UpdateUserProfileDto
    {
        [Required]
        public int Id { get; set; }
        [AllowNull]
        public string? Description { get; set; }
    }
}
