namespace e_Prescription.EmailSender
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message, bool isHtml = true);
    }
}
