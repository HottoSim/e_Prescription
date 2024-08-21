using e_Prescription.Data;
using e_Prescription.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace e_Prescription.Controllers
{
    public class EmailSenderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmailSenderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        // Assuming you have a method to get admission details by ID
        private Admission GetAdmissionById(int id)
        {
            // Fetch admission data from your database (this is just a placeholder)
            // Replace with your actual data access code.
            return _context.Admissions.Include(a => a.Patient)
                                       .Include(a => a.Patient.PatientBooking)
                                       .Include(a => a.Patient.PatientBooking.ApplicationUser)
                                       .Include(a => a.PatientsVitals)
                                       .FirstOrDefault(a => a.Id == id);
        }

        public async Task<IActionResult> EmailVitals(int admissionId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var admission = GetAdmissionById(admissionId);
            if (admission == null)
            {
                return NotFound();
            }

            var surgeonEmail = admission.Patient.PatientBooking.ApplicationUser.Email;
            var nurseEmail = user.Email;
            var password = user.PasswordHash;
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
             
            using (var smtpClient = new SmtpClient("smtp.gmail.com", 587)) // Port 587 for STARTTLS
            {
                smtpClient.EnableSsl = true; // Enable SSL/TLS
                smtpClient.Credentials = new NetworkCredential(nurseEmail, password); // Replace with your credentials

                var mailMessage = new MailMessage(nurseEmail, surgeonEmail, subject, body.ToString());
                await smtpClient.SendMailAsync(mailMessage);
            }

            return RedirectToAction("Index"); // Or wherever you want to redirect after sending the email
        }

    }
}
