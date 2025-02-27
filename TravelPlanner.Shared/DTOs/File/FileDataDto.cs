using TravelPlanner.Domain.Models;

namespace TravelPlanner.Shared.DTOs.File
{
    public record FileDataDto
    {
        public int Id { get; set; }
        public Guid FileId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long Size { get; set; }
        public DateTime UploadedAt { get; set; }
        public int? UploadedById { get; set; }
        public bool IsUploaded { get; set; } = false;
        public EntityType EntityType { get; set; } = EntityType.None;
        public int? EntityId { get; set; }
        public bool IsPublic { get; set; } = false;
    }
}
