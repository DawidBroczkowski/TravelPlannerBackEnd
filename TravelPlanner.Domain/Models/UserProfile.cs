using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelPlanner.Domain.Models
{
    public record UserProfile : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        [Required]
        public virtual ApplicationUser? User { get; set; }
        [Required]
        [MaxLength(2048)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public Guid ProfileImageId { get; set; } = Guid.Empty;
    }
}
