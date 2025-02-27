using TravelPlanner.Shared.DTOs.File;

namespace TravelPlanner.Shared.DTOs.UserProfile
{
    public record GetUserProfileDto : BaseEntityGetDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid ProfileImageId { get; set; }
        public List<FileDataDto> Files { get; set; } = new List<FileDataDto>();
        public string Description { get; set; } = string.Empty;
    }
}
