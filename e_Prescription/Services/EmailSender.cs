using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using MailKit.Net.Smtp;

namespace e_Prescription.Services
{
    public class EmailSender
    {
        private readonly string from = "baybreezeday@gmail.com";
        private readonly string password = "yscu uxlt afal fefg";

        public void SendMessage(string to, string subject, string bodyText)
        {
            MimeMessage message = new MimeMessage();

            // Pass both name and email address to MailboxAddress
            message.From.Add(new MailboxAddress("Bay Breeze", from));
            message.To.Add(new MailboxAddress("", to));  // Empty string for the name if not needed

            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = bodyText
            };

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect("smtp.gmail.com", 587);

                client.Authenticate(from, password);

                client.Send(message);

                client.Disconnect(true);
            }
        }
    }
}
