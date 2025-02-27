using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace TravelPlanner.Domain.Models
{
    public record Penalty
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public virtual ApplicationUser? User { get; set; }
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        [Required]
        public PenaltyType Type { get; set; }
        [Required]
        [MaxLength(256)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public DateTime IssuedAt { get; init; } = DateTime.UtcNow;
        [AllowNull]
        public virtual ApplicationUser? IssuedBy { get; set; }
        [ForeignKey("IssuedBy")]
        [AllowNull]
        public int? IssuedById { get; set; }
        [Required]
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime EndDate { get; set; } = DateTime.UtcNow;
        [Required]
        public bool IsPermanent { get; set; } = false;
        [AllowNull]
        public DateTime? UpdatedAt { get; set; }
        [Required]
        public bool IsDeleted { get; set; } = false;
        [NotMapped]
        public bool IsActive => !IsDeleted && !IsPermanent && StartDate <= DateTime.UtcNow && DateTime.UtcNow <= EndDate;
    }
}
