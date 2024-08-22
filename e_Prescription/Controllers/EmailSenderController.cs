using e_Prescription.Data;
using e_Prescription.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using System.Text;
using MailKit.Net.Smtp;
using MimeKit;
using e_Prescription.Models.ViewModels;

namespace e_Prescription.Controllers
{
    public class EmailSenderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public EmailSenderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
        }
        public IActionResult Index()
        {
            return View();
        }
        // Assuming you have a method to get admission details by ID
        private Admission GetAdmissionById(int id)
        {
            var admission = _context.Admissions.Include(a => a.Patient)
                                       .Include(a => a.Patient.PatientBooking)
                                       .Include(a => a.Patient.PatientBooking.ApplicationUser)
                                       .Include(a => a.PatientsVitals)
                                       .FirstOrDefault(a => a.Id == id);
            if(admission == null)
            {
                return ViewBag.ErrorMessage = "Admission not found...";
            }
            // Fetch admission data from your database (this is just a placeholder)
            // Replace with your actual data access code.
            return admission;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EmailVitals(VitalsNoteVM model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var admission = GetAdmissionById(model.AdmissionId);
            if (admission == null)
            {
                return NotFound();
            }

            var surgeonEmail = admission.Patient.PatientBooking.ApplicationUser.Email;
            var nurseEmail = user.Email;
            var subject = $"Patient Vitals for {admission.Patient.Firstname} {admission.Patient.Lastname}";

            var body = new StringBuilder();
            body.AppendLine($"Patient: {admission.Patient.Firstname} {admission.Patient.Lastname} (ID: {admission.Patient.IdNumber})");
            body.AppendLine($"Gender: {admission.Patient.Gender}");
            body.AppendLine($"Admission Date: {admission.AdmissionDate.ToLongDateString()} {admission.AdmissionDate.ToShortTimeString()}");
            body.AppendLine($"Ward and Bed: {admission.Ward?.WardName} - {admission.Bed?.BedName}");
            body.AppendLine();
            body.AppendLine("Vitals:");
            foreach (var vital in admission.PatientsVitals)
            {
                body.AppendLine($"{vital.Vitals?.VitalName}: {vital.Reading} {vital.Vitals?.Units} (Time: {vital.Time.ToShortTimeString()})");
                body.AppendLine($"Note: {vital.Note}");
                body.AppendLine($"Normal range: {vital.Vitals?.LowLimit} - {vital.Vitals?.HighLimit} {vital.Vitals?.Units}");
                body.AppendLine();
            }
            body.AppendLine($"Additional Nurse Note: {model.Note}");

            // Create the email message
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Nurse", nurseEmail));
            message.To.Add(new MailboxAddress("Surgeon", surgeonEmail));
            message.Subject = subject;

            // Use the body string as the email body
            message.Body = new TextPart("plain")
            {
                Text = body.ToString()
            };

            // Send the email using MailKit's SmtpClient
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

                // Use the provided password
                await client.AuthenticateAsync(nurseEmail, model.Password);

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

            return RedirectToAction("Index");
        }
    }
}
