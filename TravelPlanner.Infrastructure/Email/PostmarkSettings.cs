namespace TravelPlanner.Infrastructure.Email
{
    public record PostmarkSettings
    {
        public string ServerToken { get; set; } = string.Empty;
        public string FromEmail { get; set; } = string.Empty;
    }
}
