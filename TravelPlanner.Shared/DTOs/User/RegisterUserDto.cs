using System.ComponentModel.DataAnnotations;

namespace TravelPlanner.Shared.DTOs.User
{
    public record RegisterUserDto
    {
        [Required]
        [Length(3, 64)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        [MinLength(6), MaxLength(30)]
        public string Password { get; set; } = string.Empty;
    }
}
