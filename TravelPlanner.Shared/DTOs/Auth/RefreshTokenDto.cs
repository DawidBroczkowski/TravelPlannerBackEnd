namespace TravelPlanner.Shared.DTOs.Auth
{
    public record RefreshTokenDto
    {
        public int UserId { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
    }
}
