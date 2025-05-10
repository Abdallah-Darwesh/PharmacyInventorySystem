using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace PharmacyInventorySystem.Services
{
    public class FakeEmailSender : IEmailSender
    {
        private readonly ILogger<FakeEmailSender> _logger;

        public FakeEmailSender(ILogger<FakeEmailSender> logger)
        {
            _logger = logger;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // تسجل تفاصيل الإيميل في الـ log لتفقد الرابط
            _logger.LogInformation($"Sending email to {email} with subject {subject}. Message: {htmlMessage}");
            return Task.CompletedTask;
        }
    }
}
