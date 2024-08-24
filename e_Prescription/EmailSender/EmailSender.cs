using System.Net;
using System.Net.Mail;

namespace e_Prescription.EmailSender
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var mail = "baybreezeday@gmail.com";
            var pw = "ywbh axfa unmj xlbb";

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, pw)
            };

            return client.SendMailAsync(new MailMessage(from: mail, to: email, subject, message));
        }
    }
}
