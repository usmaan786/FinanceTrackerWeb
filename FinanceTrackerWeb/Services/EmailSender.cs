using Microsoft.AspNetCore.Identity.UI.Services;

namespace FinanceTrackerWeb.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Implement your email sending logic here.
            // For demo purposes, we're not actually sending an email.
            return Task.CompletedTask;
        }
    }
}
