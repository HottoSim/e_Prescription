using e_Prescription.Data;
using e_Prescription.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using e_Prescription.Models.Account;



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

        // Search for existing patients
        public IActionResult Index(string IdNumber)
        {
            // Search for the patient with the provided ID number
            var patient = _context.Patients.FirstOrDefault(p => p.IdNumber == IdNumber);

            if (patient == null)
            {
                // If no patient is found
                ViewBag.Message = "Patient not found";
                return View();
            }

            // If the patient is found, pass the patient to the view
            return View(patient);
        }

        //Surgeon appointments booked
        public async Task<IActionResult> SurgeonAppointments()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            var appointments = await _context.BookingTreatments
                .Include(a => a.PatientBooking)
                    .ThenInclude(pb => pb.Patient)
                .Include(a => a.PatientBooking)
                    .ThenInclude(pb => pb.Theatre)  // Include the Theatre model
                .Include(a => a.TreatmentCode)
                .Where(a => a.PatientBooking.SurgeonId == user.Id && a.PatientBooking.Patient.Status != "Discharged")
                .ToListAsync();

            return View(appointments);
        }



        //Add new patient for booking
        [HttpGet]
        public IActionResult AddPatient()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPatient(Patient patient)
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            ViewBag.SuccessMessage = "Patient has been added successfully...";
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

            ViewBag.getTheatre = _context.Theatre.ToList();
            ViewBag.TreatmentCodes = _context.TreatmentCodes.ToList();

            var model = new BookingTreatmentViewModel
            {
                PatientId = patientId
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> BookSurgery(BookingTreatmentViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var surgeon = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (surgeon == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients.FindAsync(model.PatientId);
            if (patient == null)
            {
                return NotFound();
            }

            var patientBooking = new PatientBooking
            {
                PatientId = model.PatientId,
                Date = model.BookingDate,
                TheatreId = model.theatreId,
                SurgeonId = surgeon.Id,
                TimeSlot = model.timeSlot,
            };

            _context.PatientBookings.Add(patientBooking);
            await _context.SaveChangesAsync(); // Save to generate the BookingId

            var bookingTreatment = new BookingTreatment
            {
                BookingId = patientBooking.BookingId, // Uses the generated BookingId
                TreatmentCodeId = model.treatmentCodeId,
            };

            _context.BookingTreatments.Add(bookingTreatment);
            await _context.SaveChangesAsync();

            //patient.IsActive = true;

            patient.Status = "Not Admitted";
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync(); // Save changes to update the patient's availability

            TempData["SuccessSurgery"] = "Surgery has been successfully booked.";
            return RedirectToAction("SurgeonAppointments");
        }


    }
}
