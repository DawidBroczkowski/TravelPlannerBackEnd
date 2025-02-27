namespace TravelPlanner.Shared.DTOs.Auth
{
    public record AuthenticationDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime AccessTokenExpiry { get; set; }
        public long Permissions { get; set; }
    }
}
