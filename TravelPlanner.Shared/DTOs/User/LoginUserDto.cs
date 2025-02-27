using System.ComponentModel.DataAnnotations;

namespace TravelPlanner.Shared.DTOs.User
{
    public record LoginUserDto
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6), MaxLength(30)]
        public string Password { get; set; } = string.Empty;
        [Required]
        public bool RememberMe { get; set; }
    }
}
