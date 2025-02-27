using System.ComponentModel.DataAnnotations;
using TravelPlanner.Domain.Models;

namespace TravelPlanner.Shared.DTOs.File
{
    public record AssignFileDto
    {
        [Required]
        public Guid FileId { get; set; }
        [Required]
        public int EntityId { get; set; }
        [Required]
        public EntityType EntityType { get; set; }
        [Required]
        public bool IsPublic { get; set; }
    }
}
