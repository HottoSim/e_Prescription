using Microsoft.AspNetCore.Mvc;
using e_Prescription.Services;
using e_Prescription.Models.ViewModels;
using e_Prescription.EmailSender;
using Microsoft.EntityFrameworkCore;
using e_Prescription.Data;

namespace e_Prescription.Controllers
{
    public class EmailSenderController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;

        public EmailSenderController(IEmailSender emailSender, ApplicationDbContext context)
        {
           this._emailSender = emailSender;
            _context = context;
        }

        [HttpGet]
        public IActionResult ComposeEmail(int admissionId)
        {
            // Fetch relevant data for the email form
            var admission = _context.Admissions
                .Include(a => a.Patient)
                .FirstOrDefault(a => a.Id == admissionId);

            if (admission == null)
            {
                return NotFound();
            }

            var model = new EmailViewModel
            {
                AdmissionId = admissionId,
                PatientName = $"{admission.Patient.Firstname} {admission.Patient.Lastname}"
            };

            return PartialView("_ComposeEmailForm", model);
        }


        [HttpPost]
        public async Task<IActionResult> SendEmail(int admissionId, string subject)
        {
            var admission = _context.Admissions
                .Include(a => a.Patient)
                .ThenInclude(p => p.PatientBooking)
                .ThenInclude(pb => pb.ApplicationUser)
                .Include(a => a.PatientsVitals)
                .FirstOrDefault(a => a.Id == admissionId);

            if (admission == null)
            {
                TempData["ErrorMessage"] = "Admission not found.";
                return RedirectToAction("ViewAdmission", "Admission", new { admissionId = admissionId });
            }

            var patient = admission.Patient;
            if (patient == null)
            {
                TempData["ErrorMessage"] = "Patient not found.";
                return RedirectToAction("ViewAdmission", "Admission", new { admissionId = admissionId });
            }

            var patientBooking = patient.PatientBooking;
            var applicationUser = patientBooking?.ApplicationUser;

            var body = $@"
        <h2>Patient Vitals for {patient.Firstname} {patient.Lastname}</h2>
        <p><strong>Gender:</strong> {patient.Gender}</p>
        <p><strong>Admission time:</strong> {admission.AdmissionDate.ToLongDateString()}, {admission.AdmissionDate.ToShortTimeString()}</p>
        <p><strong>Ward and Bed:</strong> {admission.Ward?.WardName} - {admission.Bed?.BedName}</p>";

            if (applicationUser != null)
            {
                body += $@"
            <p><strong>Nurse Responsible:</strong> {admission.ApplicationUser.FirstName} {admission.ApplicationUser.LastName}</p>
            <p><strong>Contact:</strong> {admission.ApplicationUser.ContactNumber} / {admission.ApplicationUser.Email}</p>";
            }
            else
            {
                body += $@"
            <p><strong>Nurse Responsible:</strong> Not available</p>";
            }

            body += "<p><strong>Vitals:</strong></p>";

            foreach (var vital in admission.PatientsVitals)
            {
                body += $@"
            <p><strong>{vital.Vitals?.VitalName}:</strong> {vital.Reading} {vital.Vitals?.Units} at {vital.Time.ToShortTimeString()}</p>";
            }

            // Add more details as needed

            var recipient = admission.Patient.PatientBooking.ApplicationUser.Email; // Modify as needed
            if(recipient == null)
            {
                return NotFound();
            }
            await _emailSender.SendEmailAsync(recipient, subject, body);

            TempData["SuccessMessage"] = "Email sent successfully.";
            return RedirectToAction("ViewAdmission", "Admission", new { admissionId = admissionId });
        }
    }
}
