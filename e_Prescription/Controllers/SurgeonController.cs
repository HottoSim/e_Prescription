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

        //Return admissions for the surgeon
        [HttpGet]
        public async Task<IActionResult> GetAdmission()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            var admission = await _context.Admissions
                            .Include(p => p.Patient)
                            .ThenInclude(pa => pa.PatientBooking)
                            .ThenInclude(pc => pc.ApplicationUser)
                            .Include(bd => bd.Bed)
                            .Include(wd => wd.Ward)
                            .Where(a => a.Patient.PatientBooking.ApplicationUser.Id  == user.Id)
                            .ToListAsync();
            return View(admission);
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


        public async Task<IActionResult> Create(int admissionId)
        {
            // Retrieve the admission details including patient information
            var admission = await _context.Admissions
                                .Include(a => a.Patient)
                                .FirstOrDefaultAsync(a => a.Id == admissionId);

            if (admission == null || admission.Patient == null)
            {
                // Return a 404 view if the admission or patient is not found
                return NotFound("Admission or Patient details not found.");
            }

            // Load the available medications for selection
            var medications = await _context.PharmacyMedications.ToListAsync();

            if (medications == null || !medications.Any())
            {
                ModelState.AddModelError(string.Empty, "No medications available for prescription.");
                return View(new Prescription());
            }

            // Create a new prescription object to pass to the view
            var prescription = new Prescription
            {
                AdmissionId = admissionId,
                Admission = admission,
                PresciptionDate = DateTime.Now,
                Status = "Pending",
            };

            // Pass the single prescription and list of medications to the view
            ViewBag.Medications = medications;
            return View(prescription);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Prescription model, int selectedMedicationId, int quantity, string instruction)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

             // Find the selected medication based on the passed medication ID
                var selectedMedication = await _context.PharmacyMedications
                    .FirstOrDefaultAsync(m => m.PharmacyMedicationId == selectedMedicationId);

                if (selectedMedication == null)
                {
                    ModelState.AddModelError(string.Empty, "Selected medication not found.");
                    return View(model);
                }

                // Create a new prescription item with the selected medication details
                var prescriptionItem = new PrescriptionItem
                {
                    PrescriptionId = model.PrescriptionId,
                    MedicationId = selectedMedicationId,
                    Quantity = quantity,
                    Instruction = instruction
                };

                // Save the prescription and prescription item to the database
                model.PrescriptionItems.Add(prescriptionItem);
                model.SurgeonId = user.Id;
                model.Status = "Prescribed";
                _context.Prescriptions.Add(model);
                await _context.SaveChangesAsync();

            

            // Reload available medications in case of an error
            ViewBag.Medications = await _context.PharmacyMedications.ToListAsync();
            return RedirectToAction("GetAdmission"); // Redirect to a list of prescriptions or another view
        }

    }

}

