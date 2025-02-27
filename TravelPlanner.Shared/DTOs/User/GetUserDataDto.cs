namespace TravelPlanner.Shared.DTOs.User
{
    public record GetUserDataDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.MinValue;
        public DateTime? UpdatedAt { get; set; }
        public bool IsBanned { get; set; } = false;
        public bool IsMuted { get; set; } = false;
        public DateTime? MuteEndDate { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}
