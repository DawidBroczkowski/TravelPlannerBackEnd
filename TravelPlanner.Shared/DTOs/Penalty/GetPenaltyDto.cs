using TravelPlanner.Domain.Models;

namespace TravelPlanner.Shared.DTOs.Penalty
{
    public record GetPenaltyDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public PenaltyType Type { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime IssuedAt { get; init; } = DateTime.UtcNow;
        public int? IssuedById { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime EndDate { get; set; } = DateTime.UtcNow;
        public bool IsPermanent { get; set; } = false;
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsActive => !IsDeleted && !IsPermanent && StartDate <= DateTime.UtcNow && DateTime.UtcNow <= EndDate;
    }
}
