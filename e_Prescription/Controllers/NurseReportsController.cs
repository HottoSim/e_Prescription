using e_Prescription.Data;
using e_Prescription.Models.ViewModels;
using e_Prescription.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace e_Prescription.Controllers
{
    public class NurseReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public NurseReportsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> NurseReport(DateTime startDate, DateTime endDate)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            return View();
        }
        public async Task<ActionResult> GetPatientMedicationReport(DateTime? startDate, DateTime? endDate)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            ViewBag.NurseName = user.FirstName + " " + user.LastName;

            // Set default date range if not provided
            var start = startDate ?? DateTime.MinValue;
            var end = endDate ?? DateTime.MaxValue;

            // Query using navigation properties, and sorting by AdmissionDate
            var report = await _context.Admissions
                .Where(a => a.AdmissionDate >= start && a.AdmissionDate <= end && a.NurseId == user.Id)
                .SelectMany(a => a.Prescriptions.SelectMany(p => p.PrescriptionItems.SelectMany(pi => pi.MedicationGiven.Select(mg => new PatientMedicationReportViewModel
                {
                    PatientId = a.PatientId,
                    PatientName = a.Patient.Firstname + " " + a.Patient.Lastname,
                    AdmissionDate = a.AdmissionDate,
                    MedicationName = pi.PharmacyMedication.MedicationName,
                    QuantityGiven = mg.Quantity,
                    Time = mg.Time,
                    DateRangeStart = start,
                    DateRangeEnd = end
                }))))
                .OrderBy(r => r.AdmissionDate)
                .ToListAsync();

            // Summarize medication data
            var medicineSummary = report
                .GroupBy(r => r.MedicationName)
                .Select(g => new
                {
                    Medicine = g.Key,
                    QtyAdministered = g.Sum(r => r.QuantityGiven)
                })
                .ToList();

            // Group by patient name, admission date, and include PatientId
            var groupedReport = report
                .GroupBy(r => new { r.PatientId, r.PatientName, r.AdmissionDate })
                .Select(g => new PatientMedicationGroupViewModel
                {
                    PatientId = g.Key.PatientId,
                    PatientName = g.Key.PatientName,
                    AdmissionDate = g.Key.AdmissionDate,
                    Medications = g.ToList()
                })
                .ToList();

            ViewBag.CountPatients = groupedReport.Count;
            ViewData["StartDate"] = start.ToString("d MMMM, yyyy");
            ViewData["EndDate"] = end.ToString("d MMMM, yyyy");
            ViewBag.MedicineSummary = medicineSummary;

            return View(groupedReport);
        }



        [HttpGet]
        public async Task<IActionResult> Report(int patientId)
        {
            var patient = await _context.Patients
                .Include(p => p.PatientAllergies)
                    .ThenInclude(pa => pa.ActiveIngredient)
                .Include(p => p.PatientConditions)
                    .ThenInclude(pc => pc.ChronicCondition)
                .Include(p => p.PatientMedications)
                    .ThenInclude(pm => pm.Medication)
                .Include(p => p.PatientBooking)
                    .ThenInclude(pb => pb.Theatre)
                .FirstOrDefaultAsync(p => p.PatientId == patientId);

            if (patient == null)
            {
                return NotFound("patient");
            }

            var admission = await _context.Admissions
                .Include(a => a.Ward)
                .Include(a => a.Bed)
                .Include(a => a.PatientsVitals)
                    .ThenInclude(v => v.Vitals)
                .Where(a => a.Patient.PatientId == patientId && a.IsDischarged)
                .FirstOrDefaultAsync();

            if (admission == null)
            {
                return NotFound("Admission");
            }

            var discharge = await _context.Discharges
                .Include(d => d.ApplicationUser)
                .Where(d => d.AdmissionId == admission.Id)
                .FirstOrDefaultAsync();

            if (discharge == null)
            {
                return NotFound("Discharge");
            }

            var prescriptions = await _context.Prescriptions
                .Include(p => p.ApplicationUser)
                .Include(p => p.Admission)
                .Where(p => p.AdmissionId == admission.Id)
                .ToListAsync();

            var prescriptionItems = await _context.PrescriptionItems
                .Include(pi => pi.PharmacyMedication)
                .Where(pi => prescriptions.Select(p => p.PrescriptionId).Contains(pi.PrescriptionId))
                .ToListAsync();

            var viewModel = new PatientReportViewModel
            {
                PatientId = patient.PatientId,
                Firstname = patient.Firstname,
                Lastname = patient.Lastname,
                Gender = patient.Gender,
                IdNumber = patient.IdNumber,
                DateOfBirth = patient.DateOfBirth,
                Status = patient.Status,
                Allergies = patient.PatientAllergies.ToList(),
                Conditions = patient.PatientConditions.ToList(),
                Medications = patient.PatientMedications.ToList(),
                Booking = patient.PatientBooking,
                Admission = admission,
                Discharge = discharge,
                Vitals = admission.PatientsVitals.ToList(),
                Prescriptions = prescriptions,
                PrescriptionItems = prescriptionItems
            };

            return View(viewModel);
        }
    }
}
