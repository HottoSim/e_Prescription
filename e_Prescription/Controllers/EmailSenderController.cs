using e_Prescription.Data;
using e_Prescription.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using System.Text;
using MimeKit;
using e_Prescription.Models.ViewModels;
using Org.BouncyCastle.Asn1.IsisMtt.X509;

namespace e_Prescription.Controllers
{
    public class EmailSenderController : Controller
    {
        private readonly ApplicationDbContext _context; // Your EF context
        private readonly UserManager<ApplicationUser> _userManager;

        public EmailSenderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult ComposeEmail(int admissionId)
        {
            var user = _userManager.GetUserAsync(User).Result; // Get logged-in user
            if (user == null)
            {
                // Handle the case where the user is not found
                return RedirectToAction("Error", "Home");
            }

            var model = new EmailViewModel
            {
                FromEmail = user.Email, // Auto-fill from the logged-in user
                AdmissionId = admissionId // Populate the model with the selected admission ID
            };

            // Fetch data from the Admission table and populate the Message
            var admission = _context.Admissions
                .Include(a => a.Patient) // Ensure related entities are included
                .Include(a => a.Bed) // Ensure related entities are included
                .FirstOrDefault(a => a.Id == admissionId);

            if (admission != null)
            {
                // Check if related properties are null before accessing them
                var patientName = admission.Patient?.Firstname ?? "Unknown";
                var bedName = admission.Bed?.BedName ?? "Unknown";

                model.Message = $"Patient Name: {patientName}\nAdmission Date: {admission.AdmissionDate}\nDetails: {bedName}";
            }
            else
            {
                // Handle the case where admission is not found
                TempData["ErrorMessage"] = "Admission not found.";
                return RedirectToAction("Error", "Home");
            }

            return View(model);
        }


        public IActionResult SendEmail(EmailViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.ToEmail))
            {
                TempData["ErrorMessage"] = "Recipient email is required.";
                return RedirectToAction("ComposeEmail", new { admissionId = model.AdmissionId });
            }

            if (string.IsNullOrWhiteSpace(model.Subject) || string.IsNullOrWhiteSpace(model.Message))
            {
                TempData["ErrorMessage"] = "Subject and Message are required.";
                return RedirectToAction("ComposeEmail", new { admissionId = model.AdmissionId });
            }

            try
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(model.FromEmail),
                    Subject = model.Subject,
                    Body = model.Message,
                    IsBodyHtml = false // Set to true if the body contains HTML
                };

                mailMessage.To.Add(model.ToEmail);

                using (var smtpClient = new SmtpClient("smtp.gmail.com", 587)) // TLS Port
                {
                    smtpClient.Credentials = new NetworkCredential("your-email@gmail.com", "your-app-password");
                    smtpClient.EnableSsl = true; // Enable SSL/TLS
                    smtpClient.Send(mailMessage);
                }

                return RedirectToAction("SuccessView"); // Redirect to a success view
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while sending the email: {ex.Message}";
                return RedirectToAction("ComposeEmail", new { admissionId = model.AdmissionId });
            }
        }

    }
}
