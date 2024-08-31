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

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Report(int id)
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
                .FirstOrDefaultAsync(p => p.PatientId == id);

            if (patient == null)
            {
                return NotFound("patient");
            }

            var admission = await _context.Admissions
                .Include(a => a.Ward)
                .Include(a => a.Bed)
                .Include(a => a.PatientsVitals)
                    .ThenInclude(v => v.Vitals)
                .Where(a => a.Patient.PatientId == id && a.IsDischarged)
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
