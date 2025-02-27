using Microsoft.Extensions.Options;
using PostmarkDotNet;
using TravelPlanner.Infrastructure.Email.Interfaces;

namespace TravelPlanner.Infrastructure.Email
{
    public class PostmarkEmailService : IEmailService
    {
        private readonly PostmarkSettings _postmarkSettings;

        public PostmarkEmailService(IOptions<PostmarkSettings> postmarkSettings)
        {
            _postmarkSettings = postmarkSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string messageBody)
        {
            var client = new PostmarkClient(_postmarkSettings.ServerToken);

            var message = new PostmarkMessage
            {
                From = _postmarkSettings.FromEmail,
                To = _postmarkSettings.FromEmail, // Same email for testing, postmark allows only emails within the same domain. Needs to be changed later.
                Subject = subject,
                HtmlBody = messageBody
            };

            var response = await client.SendMessageAsync(message);

            if (response.Status != PostmarkStatus.Success)
            {
                throw new Exception($"Error sending email: {response.Message}");
            }
        }
    }

}
