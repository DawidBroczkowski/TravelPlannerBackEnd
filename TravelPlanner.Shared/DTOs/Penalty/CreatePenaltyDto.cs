using System.ComponentModel.DataAnnotations;
using TravelPlanner.Domain.Models;

namespace TravelPlanner.Shared.DTOs.Penalty
{
    public record CreatePenaltyDto
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public PenaltyType Type { get; set; }
        [Required]
        [MaxLength(256)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime EndDate { get; set; } = DateTime.UtcNow;
        [Required]
        public bool IsPermanent { get; set; } = false;
    }
}
