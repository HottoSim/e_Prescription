using e_Prescription.Data;
using e_Prescription.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using e_Prescription.Models.Account;
using AspNetCore;


namespace e_Prescription.Controllers
{
    public class SurgeonController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SurgeonController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //Search for existing patients
        public IActionResult Index(int patientId)
        {
            return View();
        }

        //Surgeon booked patients
        public async Task<IActionResult> SurgeonAppointments()
        {
            var user = await _userManager.GetUserAsync(User);

            if(user == null)
            {
                return NotFound();
            }

            var appointments = _context.BookingTreatments
                               .Where(a => a.PatientBooking.SurgeonId == user.Id && a.PatientBooking.Patient.IsActive)
                               .ToList();

            return View(appointments);
        }

        //Add new patient for booking
        [HttpPost]
        public IActionResult AddPatient()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPatient(Patient patient)
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            TempData["SucessMessage"] = "Patient has been added successfully...";
            return View(patient);
        }

        //Surgery booking 
        [HttpGet]
        public IActionResult BookSurgery(int patientId)
        {
            var patient = _context.Patients.Find(patientId);
            if (patient == null)
            {
                return NotFound();
            }

            ViewBag.TreatmentCodes = _context.TreatmentCodes.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BookSurgery(BookingTreatmentViewModel model, int patientId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var surgeon = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (surgeon == null)
            {
                return NotFound();
            }

            var patientBooking = new PatientBooking
            {
                Date = model.BookingDate,
                TheatreId = model.theatreId,
                SurgeonId = surgeon.Id,
                TimeSlot = model.timeSlot,

            };

            _context.PatientBookings.Add(patientBooking);
            await _context.SaveChangesAsync(); // Save to generate the BookingId

            var bookingTreatment = new BookingTreatment
            {
                BookingId = patientBooking.BookingId, //Uses the generated BookingId
                TreatmentCodeId = model.treatmentCodeId,
            };

            _context.BookingTreatments.Add(bookingTreatment);
            await _context.SaveChangesAsync();

            var patient = _context.Patients.Find(patientId);
            if (patient == null)
            {
                return NotFound();
            }

            patient.IsActive = true;
            await _context.SaveChangesAsync(); // Save changes to update the patient's availability

            ViewBag.TreatmentCodes = _context.TreatmentCodes.ToList();

            TempData["SuccessMessage"] = "Patient has been sucessfully created...";
            return RedirectToAction("Index");
        }
    }
}
