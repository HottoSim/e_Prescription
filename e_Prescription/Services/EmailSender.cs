using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace e_Prescription.Services
{
    public class EmailSender :IEmailSender
    {

        // Ensure the constructor is public
        public EmailSender()
        {
            // Any initialization code can go here
        }

        // Implement the SendEmailAsync method
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Implement your email sending logic here.
            // For example, using an SMTP client or a third-party service like SendGrid.
            return Task.CompletedTask; // Replace with actual implementation.
        }
    }
}
