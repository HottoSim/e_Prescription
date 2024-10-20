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
                           .ThenInclude(v => v.Vitals) // Ensure Vitals are included
                           .Include(a => a.Ward) // Include Ward
                           .Include(a => a.Bed) // Include Bed
                           .Include(a => a.ApplicationUser) // Include Nurse (ApplicationUser)
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
            var applicationUser2 = admission.ApplicationUser;

            // Build email content using a table
            var body = $@"
    <h2>Patient Vitals for {patient.Firstname} {patient.Lastname} - {patient.IdNumber}</h2>
    <table border='1' cellpadding='5' cellspacing='0' style='border-collapse: collapse; width: 100%;'>
        <tr>
            <th colspan='2' style='text-align: left;'>Patient Details</th>
        </tr>
        <tr>
            <td><strong>Full Name:</strong></td>
            <td>{patient.Firstname} {patient.Lastname}</td>
        </tr>
        <tr>
            <td><strong>Gender:</strong></td>
            <td>{patient.Gender}</td>
        </tr>
         <tr>
            <td><strong>Weight and Height:</strong></td>
            <td>{admission.Weight} kg and {admission.Height} cm</td>
        </tr>
        <tr>
            <td><strong>BMI:</strong></td>
            <td>{admission.BMI.Value.ToString("F2")} - normal range (18.5 - 24.9)</td>
        </tr>
        <tr>
            <td><strong>Admission Time:</strong></td>
            <td>{admission.AdmissionDate.ToLongDateString()}, {admission.AdmissionDate.ToShortTimeString()}</td>
        </tr>
        <tr>
            <td><strong>Ward and Bed:</strong></td>
            <td>{admission.Ward?.WardName} - {admission.Bed?.BedName}</td>
        </tr>
        <tr>
            <td><strong>Nurse Responsible:</strong></td>
            <td>{(applicationUser2 != null ? $"{applicationUser2.FirstName} {applicationUser2.LastName}" : "Not Available")}</td>
        </tr>
        <tr>
            <td><strong>Contact:</strong></td>
            <td>{(applicationUser2 != null ? $"{applicationUser2.ContactNumber} / {applicationUser2.Email}" : "N/A")}</td>
        </tr>
    </table>
    <br/>
    <h3>Vitals</h3>
    <table border='1' cellpadding='5' cellspacing='0' style='border-collapse: collapse; width: 100%;'>
        <tr>
            <th>Vital Name</th>
            <th>Reading</th>
            <th>Units</th>
            <th>Time</th>
        </tr>";

            foreach (var vital in admission.PatientsVitals)
            {
                body += $@"
        <tr>
            <td>{vital.Vitals?.VitalName}</td>
            <td>{vital.Reading}</td>
            <td>{vital.Vitals?.Units}</td>
            <td>{vital.Time.ToShortTimeString()}</td>
        </tr>";
            }

            body += "</table>";

            var recipient = patientBooking?.ApplicationUser?.Email;
            if (recipient == null)
            {
                return NotFound();
            }

            // Send the email with the formatted HTML table
            await _emailSender.SendEmailAsync(recipient, subject, body, isHtml: true);

            TempData["SuccessMessage"] = "Email sent successfully.";
            return RedirectToAction("ViewAdmission", "Admission", new { admissionId = admissionId });
        }


    }
}
