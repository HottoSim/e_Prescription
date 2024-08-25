using System.Net;
using System.Net.Mail;

namespace e_Prescription.EmailSender
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message, bool isHtml = true)
        {
            var mail = "baybreezeday@gmail.com";
            var pw = "ywbh axfa unmj xlbb";

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, pw)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(mail),
                Subject = subject,
                Body = message,
                IsBodyHtml = isHtml // This ensures that the email body is treated as HTML
            };

            mailMessage.To.Add(email);

            return client.SendMailAsync(mailMessage);
        }
    }

}
