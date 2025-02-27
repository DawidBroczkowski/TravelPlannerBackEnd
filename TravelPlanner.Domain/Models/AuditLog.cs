using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace TravelPlanner.Domain.Models
{
    public record AuditLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "varchar(64)")]
        public string TableName { get; set; } = string.Empty;
        [Required]
        public int RecordId { get; set; }
        [Required]
        [Column(TypeName = "varchar(10)")]
        public string OperationType { get; set; } = string.Empty; // "Insert", "Update", "Delete"
        [AllowNull]
        [Column(TypeName = "nvarchar(MAX)")]
        public string? OldValues { get; set; } // JSON representation of old values
        [AllowNull]
        [Column(TypeName = "nvarchar(MAX)")]
        public string? NewValues { get; set; } // JSON representation of new values
        [AllowNull]
        public virtual ApplicationUser? ChangedBy { get; set; }
        [AllowNull]
        [ForeignKey("ChangedBy")]
        public int? ChangedById { get; set; }
        [Required]
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    }
}
