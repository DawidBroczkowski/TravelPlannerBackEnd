using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using TravelPlanner.Domain.Models.Trails;

namespace TravelPlanner.Domain.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        [Required]
        [Length(3, 64)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(44)]
        public string RefreshToken { get; set; } = string.Empty;
        [AllowNull]
        public DateTime? RefreshTokenExpiry { get; set; }
        [AllowNull]
        public Guid? Jti { get; set; }
        [AllowNull]
        public DateTime? JtiExpiry { get; set; }
        [AllowNull]
        public DateTime? LastLogin { get; set; }
        [Required]
        public virtual List<Penalty> Penalties { get; set; } = new List<Penalty>();
        [Required]
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
        [AllowNull]
        public DateTime? UpdatedAt { get; set; }
        [Required]
        public bool IsDeleted { get; set; } = false;
        [Required]
        public virtual List<FileData> Files { get; set; } = new List<FileData>();
        [AllowNull]
        public virtual UserProfile? UserProfile { get; set; }
        [Required]
        public virtual List<Trail> Trails { get; set; } = new List<Trail>();
    }
}
