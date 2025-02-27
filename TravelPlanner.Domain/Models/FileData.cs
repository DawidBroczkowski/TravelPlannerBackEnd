using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace TravelPlanner.Domain.Models
{
    public record FileData
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public Guid FileId { get; set; }
        [Required]
        public string FileName { get; set; } = string.Empty;
        [Required]
        public string Extension { get; set; } = string.Empty;
        [Required]
        public string ContentType { get; set; } = string.Empty;
        [Required]
        public long Size { get; set; }
        [Required]
        public DateTime UploadedAt { get; set; }
        [AllowNull]
        public virtual ApplicationUser? UploadedBy { get; set; }
        [AllowNull]
        [ForeignKey("UploadedBy")]
        public int? UploadedById { get; set; }
        [Required]
        public bool IsUploaded { get; set; } = false;
        [Required]
        public EntityType EntityType { get; set; } = EntityType.None;
        [AllowNull]
        public int? EntityId { get; set; }
        [Required]
        public bool IsPublic { get; set; } = false;
    }
}
