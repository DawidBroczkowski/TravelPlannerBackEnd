using System.ComponentModel.DataAnnotations;

namespace TravelPlanner.Shared.DTOs.User
{
    public record ResetPasswordDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string Token { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string NewPassword { get; set; } = string.Empty;
    }
}
