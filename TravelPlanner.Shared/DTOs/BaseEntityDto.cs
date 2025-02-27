namespace TravelPlanner.Shared.DTOs
{
    public abstract record BaseEntityGetDto
    {
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsPublic { get; set; }
    }
}
