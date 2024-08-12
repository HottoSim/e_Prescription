using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using e_Prescription.Data;
using e_Prescription.Migrations;
using e_Prescription.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace e_Prescription.Controllers
{
    [Authorize(Roles = "Nurse")]
    public class AdmissionController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public AdmissionController(ApplicationDbContext _context, UserManager<ApplicationUser> _userManager)
        {
            context = _context;
            userManager = _userManager;
        }

        //Nurse Landing page
        public async Task< IActionResult> NurseLandingPage()
        {
            var user = await userManager.GetUserAsync(User);
            var nurseName = $"{user.FirstName} {user.LastName}"; // Fetch full name

            // Fetch the number of patients admitted by the current nurse
            var admittedPatientsCount = context.Admissions
                .Count(a => a.NurseId == user.Id && !a.IsDischarged);


            // Fetch the number of patients booked for admission
            var bookedPatientsCount = context.BookingTreatments
                .Count(b => b.PatientBooking.Patient.IsActive);

            var model = new NurseLandingPageViewModel
            {
                NurseName = nurseName,
                AdmittedPatientsCount = admittedPatientsCount,
                BookedPatientsCount = bookedPatientsCount,

                AdmittedCount = context.Admissions.Where(a => !a.IsDischarged).Count(),
                DischargedCount = context.Admissions.Where(a => a.IsDischarged).Count(),
                BookedPatients = context.BookingTreatments.Where(p => p.PatientBooking.Patient.IsActive).Count()

            };

            return View(model);
        }

        public IActionResult Index(string searchIdNumber, DateTime? searchDate, string sortOrder)
        {
            var bookings = context.BookingTreatments
                                .Where(p => p.PatientBooking.Patient.IsActive)
                                .Include(pb => pb.PatientBooking.Patient)
                                .Include(t => t.PatientBooking.Theatre)
                                .Include(t => t.TreatmentCode)
                                .Include(pb => pb.PatientBooking.ApplicationUser)
                                .AsQueryable();

            if (!string.IsNullOrEmpty(searchIdNumber))
            {
                bookings = bookings.Where(b => b.PatientBooking.Patient.IdNumber == searchIdNumber);
            }

            if (searchDate.HasValue)
            {
                bookings = bookings.Where(b => b.PatientBooking.Date.Date == searchDate.Value.Date);
            }

            switch (sortOrder)
            {
                case "asc":
                    bookings = bookings.OrderBy(b => b.PatientBooking.Date);
                    break;
                case "desc":
                    bookings = bookings.OrderByDescending(b => b.PatientBooking.Date);
                    break;
                default:
                    bookings = bookings.OrderBy(b => b.PatientBooking.Date);
                    break;
            }

            return View(bookings.ToList());
        }

        [HttpGet]
        public IActionResult Contacts(int patientId)
        {
            var provinces = context.Provinces.ToList();
            var cities = new List<City>
    {
        new City()
        {
            CityId = 0,
            CityName = "--Select City--"
        }
    };

            ViewBag.Provinces = new SelectList(provinces, "ProvinceId", "ProvinceName");
            ViewBag.Cities = new SelectList(cities, "CityId", "CityName");

            var patient = context.Patients.Find(patientId);
            if (patient == null)
            {
                return NotFound();
            }

            ViewBag.PatientName = $"{patient.Firstname} {patient.Lastname}";

            var contactDetail = context.ContactDetails.FirstOrDefault(c => c.PatientId == patientId);
            if (contactDetail != null)
            {
                // Populate ViewBag for existing contact detail
                ViewBag.SelectedCityId = contactDetail.CityId;
                ViewBag.SelectedSuburbId = contactDetail.SuburbId;
                return View(contactDetail);
            }

            return View(new ContactDetail { PatientId = patientId });
        }

        [HttpPost]
        public IActionResult Contacts(int patientId, ContactDetail contactDetail)
        {
            var existingContactDetail = context.ContactDetails.FirstOrDefault(c => c.PatientId == patientId);
            if (existingContactDetail != null)
            {
                // Update existing contact detail
                existingContactDetail.CellphoneNumber = contactDetail.CellphoneNumber;
                existingContactDetail.Email = contactDetail.Email;
                existingContactDetail.ProvinceId = contactDetail.ProvinceId;
                existingContactDetail.CityId = contactDetail.CityId;
                existingContactDetail.SuburbId = contactDetail.SuburbId;
                existingContactDetail.StreetAddress = contactDetail.StreetAddress;
            }
            else
            {
                // Add new contact detail
                context.ContactDetails.Add(contactDetail);
            }

            context.SaveChanges();

            var patient = context.Patients.Find(patientId);
            if (patient == null)
            {
                return NotFound();
            }

            //patient.IsActive = false;
           // context.SaveChanges(); // Save changes to update the patient's availability

            ViewBag.PatientName = $"{patient.Firstname} {patient.Lastname}";

            return RedirectToAction("Admission", "Admission", new { patientId = contactDetail.PatientId });
        }

        //Get City by the selected Province
        public JsonResult GetCityByProvinceId(int provinceId)
        {
            var city = context.Cities.Where(a => a.ProvinceId == provinceId).ToList();
            return Json(city);
        }
        //Get Suburb by the selected city
        public JsonResult GetSuburbByCityId(int cityId)
        {
            var suburbs = context.Suburbs.Where(s => s.CityId == cityId).ToList();
            return Json(suburbs);
        }

        // GET: AddVitals
        [HttpGet]
        public IActionResult AddVitals(int admissionId)
        {
            var admission = context.Admissions.FirstOrDefault(a => a.Id == admissionId);
            if (admission == null)
            {
                return NotFound();
            }

            var newAdmission = new Admission
            {
                Id = admission.Id,
                PatientsVitals = new List<PatientsVitals> { new PatientsVitals { AdmissionId = admissionId } }
            };

            List<SelectListItem> vitalNames = GetVitalNames();
            ViewBag.VitalNames = vitalNames;


            // Populate ViewBag.VitalNames with the available vital names
            ViewBag.VitalNames = context.Vital
                                         .Select(v => new SelectListItem { Value = v.VitalId.ToString(), Text = v.VitalName })
                                         .ToList();

            return View(newAdmission);
        }

        // POST: AddVitals
        [HttpPost]
        public IActionResult AddVitals(Admission model)
        {
            var admission = context.Admissions
                .Include(a => a.Patient) // Include Patient data
                .Include(a => a.PatientsVitals)
                .FirstOrDefault(a => a.Id == model.Id);

            if (admission == null)
            {
                return NotFound();
            }

            try
            {
                List<string> notifications = new List<string>();

                foreach (var vital in model.PatientsVitals)
                {
                    var vitalInfo = context.Vital.Find(vital.VitalId);
                    if (vitalInfo != null)
                    {
                        // Check if the reading is in the "150/60" blood pressure format
                        if (System.Text.RegularExpressions.Regex.IsMatch(vital.Reading, @"^\d{2,3}\/\d{2,3}$"))
                        {
                            var bloodPressureParts = vital.Reading.Split('/');
                            if (bloodPressureParts.Length == 2 &&
                                double.TryParse(bloodPressureParts[0], out double systolic) &&
                                double.TryParse(bloodPressureParts[1], out double diastolic))
                            {
                                // Split the blood pressure limit ranges
                                var limitParts = vitalInfo.LowLimit.Split('/');
                                double lowSystolic = double.Parse(limitParts[0]);
                                double lowDiastolic = double.Parse(limitParts[1]);

                                limitParts = vitalInfo.HighLimit.Split('/');
                                double highSystolic = double.Parse(limitParts[0]);
                                double highDiastolic = double.Parse(limitParts[1]);

                                // Compare both systolic and diastolic values
                                bool systolicInRange = systolic >= lowSystolic && systolic <= highSystolic;
                                bool diastolicInRange = diastolic >= lowDiastolic && diastolic <= highDiastolic;

                                if (!systolicInRange || !diastolicInRange)
                                {
                                    notifications.Add($"The blood pressure reading for {vitalInfo.VitalName} is out of range: {systolic}/{diastolic} {vitalInfo.Units} (Normal range: {vitalInfo.LowLimit} - {vitalInfo.HighLimit} {vitalInfo.Units})");
                                    vital.Note = "Out of range";
                                }
                                else
                                {
                                    vital.Note = "Normal";
                                }
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, $"Invalid format for the reading of {vitalInfo.VitalName}. Please check the input.");
                                return View(model);
                            }
                        }
                        // Handle normal readings with a range like "34-37"
                        else if (System.Text.RegularExpressions.Regex.IsMatch(vital.Reading, @"^\d+(\.\d+)?$"))
                        {
                            if (double.TryParse(vitalInfo.LowLimit, out double lowLimit) &&
                                double.TryParse(vitalInfo.HighLimit, out double highLimit) &&
                                double.TryParse(vital.Reading, out double normalReading))
                            {
                                if (normalReading < lowLimit || normalReading > highLimit)
                                {
                                    notifications.Add($"The reading for {vitalInfo.VitalName} is out of range: {normalReading} {vitalInfo.Units} (Normal range: {vitalInfo.LowLimit} - {vitalInfo.HighLimit} {vitalInfo.Units})");
                                    vital.Note = "Out of range";
                                }
                                else
                                {
                                    vital.Note = "Normal";
                                }
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, $"Invalid format for the reading of {vitalInfo.VitalName}. Please check the input.");
                                return View(model);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, $"Invalid format for the reading of {vitalInfo.VitalName}. Please check the input.");
                            return View(model);
                        }

                        vital.AdmissionId = admission.Id; // Ensure AdmissionId is set
                        context.PatientsVitals.Add(vital);
                    }
                }

                context.SaveChanges();

                if (notifications.Count > 0)
                {
                    ViewBag.Notifications = notifications;
                }
                else
                {
                    ViewBag.AlertMessage = "Vitals seem to be normal!";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error: Invalid format entered...";
            }

            ViewBag.VitalNames = context.Vital
                .Select(v => new SelectListItem { Value = v.VitalId.ToString(), Text = v.VitalName })
                .ToList();

            return View(admission); // Return the updated admission model
        }



        [HttpGet]
        public ActionResult Admission(int patientId)
        {
            var admission = new Admission();
            admission.PatientsVitals.Add(new PatientsVitals() { PatientVitalId = 1 });

            var patient = context.Patients.Find(patientId);
            if (patient == null)
            {
                return NotFound();
            }

            var wards = context.Wards.ToList();
            var beds = new List<Bed>
            {
                new Bed()
                {
                    BedId = 0, BedName = "--Select Bed--"
                }
            };

            ViewBag.Wards = new SelectList(wards, "WardId", "WardName");
            ViewBag.Beds = new SelectList(beds, "BedId", "BedName");
            ViewBag.PatientName = $"{patient.Firstname} {patient.Lastname}";
            ViewBag.PatientId = patientId;
            ViewBag.Vitals = context.Vital.ToList();
            ViewBag.Notifications = null;  // Initialize to avoid null reference
            ViewBag.AlertMessage = null;  // Initialize to avoid null reference
            ViewBag.ErrorMessage = null;

            return View(admission);
        }

        [HttpPost]
        public async Task<IActionResult> Admission(int patientId, Admission admission)
        {
            admission.PatientId = patientId;

            try
            {
                


                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var nurse = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (nurse == null)
                {
                    return NotFound();
                }
                admission.NurseId = nurse.Id;

                var notifications = new List<string>();
                foreach (var patientVital in admission.PatientsVitals)
                {
                    var vital = context.Vital.Find(patientVital.VitalId);
                    if (vital != null)
                    {
                        // Check if the reading is in the "150/60" blood pressure format
                        if (System.Text.RegularExpressions.Regex.IsMatch(patientVital.Reading, @"^\d{2,3}\/\d{2,3}$"))
                        {
                            var bloodPressureParts = patientVital.Reading.Split('/');
                            if (bloodPressureParts.Length == 2 &&
                                double.TryParse(bloodPressureParts[0], out double systolic) &&
                                double.TryParse(bloodPressureParts[1], out double diastolic))
                            {
                                // Split the blood pressure limit ranges
                                var limitParts = vital.LowLimit.Split('/');
                                double lowSystolic = double.Parse(limitParts[0]);
                                double lowDiastolic = double.Parse(limitParts[1]);

                                limitParts = vital.HighLimit.Split('/');
                                double highSystolic = double.Parse(limitParts[0]);
                                double highDiastolic = double.Parse(limitParts[1]);

                                // Compare both systolic and diastolic values
                                bool systolicInRange = systolic >= lowSystolic && systolic <= highSystolic;
                                bool diastolicInRange = diastolic >= lowDiastolic && diastolic <= highDiastolic;

                                if (!systolicInRange || !diastolicInRange)
                                {
                                    notifications.Add($"The blood pressure reading for {vital.VitalName} is out of range: {systolic}/{diastolic} {vital.Units} (Normal range: {vital.LowLimit} - {vital.HighLimit} {vital.Units})");
                                    patientVital.Note = "Out of range";
                                }
                                else
                                {
                                    patientVital.Note = "Normal";
                                }
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, $"Invalid format for the blood pressure reading of {vital.VitalName}. Please check the input.");
                                return View(admission);
                            }
                        }
                        // Handle normal readings with a range like "34-37"
                        else if (System.Text.RegularExpressions.Regex.IsMatch(patientVital.Reading, @"^\d+(\.\d+)?$"))
                        {
                            if (double.TryParse(vital.LowLimit, out double lowLimit) &&
                                double.TryParse(vital.HighLimit, out double highLimit) &&
                                double.TryParse(patientVital.Reading, out double normalReading))
                            {
                                if (normalReading < lowLimit || normalReading > highLimit)
                                {
                                    notifications.Add($"The reading for {vital.VitalName} is out of range: {normalReading} {vital.Units} (Normal range: {vital.LowLimit} - {vital.HighLimit} {vital.Units})");
                                    patientVital.Note = "Out of range";
                                }
                                else
                                {
                                    patientVital.Note = "Normal";
                                }
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, $"Invalid limit format for {vital.VitalName}. Please check the input.");
                                return View(admission);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, $"Invalid format for the reading of {vital.VitalName}. Please check the input.");
                            return View(admission);
                        }

                        context.PatientsVitals.Add(patientVital);
                    }
                }

                context.Admissions.Add(admission);
                await context.SaveChangesAsync();

                var patient = context.Patients.Find(patientId);
                if (patient == null)
                {
                    return NotFound();
                }

                patient.IsActive = false;
                context.SaveChanges(); // Save changes to update the patient's availability

                var bed = context.Beds.Find(admission.BedId);
                if (bed == null)
                {
                    return NotFound();
                }
                bed.IsAvailable = false;
                context.SaveChanges();

                if (notifications.Count > 0)
                {
                    ViewBag.Notifications = notifications;
                }
                else
                {
                    ViewBag.AlertMessage = "Vitals seem to be normal!";
                }

                return View(admission);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error: Invalid format entered...";
                return View(admission);
            }
        }



        // Get Bed By Ward
        public JsonResult GetBedByWardId(int wardId)
        {
            var beds = context.Beds.Where(b => b.WardId == wardId && b.IsAvailable).ToList();
            return Json(beds);
        }

        public List<SelectListItem> GetVitalNames()
        {
            var vitals = context.Vital.ToList();

            // Create a list of SelectListItem objects
            var vitalNames = vitals.Select(v => new SelectListItem
            {
                Text = v.VitalName,
                Value = v.VitalId.ToString() // Assuming VitalId is the primary key of the Vital entity
            }).ToList();

            return vitalNames;
        }

        // Patient History
        public IActionResult History(int patientId)
        {
            var patient = context.Patients.Find(patientId);

            if (patient == null)
            {
                return NotFound();
            }

            var viewModel = new PatientHistory
            {
                PatientId = patientId,
                Patient = patient
            };

            ViewBag.PatientName = $"{patient.Firstname} {patient.Lastname}";
            ViewBag.PatientId = patientId; // Set PatientId in ViewBag
            ViewBag.Allergies = new SelectList(context.ActiveIngredients, "ActiveIngredientId", "IngredientName");
            ViewBag.Conditions = new SelectList(context.ChronicConditions, "ChronicCondotionId", "ConditionName");
            ViewBag.Medications = new SelectList(context.Medications, "MedicationId", "MedicationName");

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult History(PatientHistory viewModel)
        {
            // Add the patient allergies to the database
            foreach (var allergyId in viewModel.SelectedAllergies)
            {
                context.PatientAllergies.Add(new PatientAllergies { PatientId = viewModel.PatientId, ActiveIngredientId = allergyId });
            }

            // Add the patient conditions to the database
            foreach (var conditionId in viewModel.SelectedConditions)
            {
                context.PatientConditions.Add(new PatientCondition { PatientId = viewModel.PatientId, ChronicConditionId = conditionId });
            }

            // Add the patient medications to the database
            foreach (var medicationId in viewModel.SelectedMedications)
            {
                context.PatientMedications.Add(new PatientMedication { PatientId = viewModel.PatientId, MedicationId = medicationId });
            }

            context.SaveChanges();

            var patient = context.Patients.Find(viewModel.PatientId);

            if (patient == null)
            {
                return NotFound();
            }
            viewModel.Patient = patient;

            ViewBag.PatientName = $"{patient.Firstname} {patient.Lastname}";
            ViewBag.PatientId = viewModel.PatientId; // Set PatientId in ViewBag
            ViewBag.Allergies = new SelectList(context.ActiveIngredients, "ActiveIngredientId", "IngredientName");
            ViewBag.Conditions = new SelectList(context.ChronicConditions, "ChronicCondotionId", "ConditionName");
            ViewBag.Medications = new SelectList(context.Medications, "MedicationId", "MedicationName");
            TempData["AlertMessage"] = $"{patient.Firstname} {patient.Lastname} has been admitted successfully!";
            return View(viewModel); // Or any appropriate action
        }

        //Retrieve all admitted patients by the Nurse
        public async Task<IActionResult> NursePatients(string patientId, string sortOrder)
        {
            var user = await userManager.GetUserAsync(User);

            // Query for admitted patients
            var admittedPatientsQuery = context.Admissions
                .Where(a => a.NurseId == user.Id && !a.IsDischarged)
                .Include(a => a.Patient)
                .Include(a => a.ApplicationUser)
                .Include(a => a.Ward)
                .Include(a => a.Bed)
                .AsQueryable();  // Ensure queryable for further querying

            // Filter by patient ID if provided
            if (!string.IsNullOrEmpty(patientId))
            {
                admittedPatientsQuery = admittedPatientsQuery.Where(a => a.Patient.IdNumber.Contains(patientId));
            }

            // Sorting logic
            ViewData["FirstNameSortParam"] = sortOrder == "FirstName" ? "FirstName_desc" : "FirstName";
            switch (sortOrder)
            {
                case "FirstName_desc":
                    admittedPatientsQuery = admittedPatientsQuery.OrderByDescending(a => a.Patient.Firstname);
                    break;
                case "FirstName":
                    admittedPatientsQuery = admittedPatientsQuery.OrderBy(a => a.Patient.Firstname);
                    break;
                default:
                    admittedPatientsQuery = admittedPatientsQuery.OrderBy(a => a.Patient.IdNumber);
                    break;
            }

            var admittedPatients = await admittedPatientsQuery.ToListAsync();

            return View(admittedPatients);
        }


        //Retrieve Patient admission from the database
        public async Task<IActionResult> ViewAdmission(string patientId, int admissionId)
        {
            if (string.IsNullOrEmpty(patientId) && admissionId == 0)
            {
                return View(new List<Admission>());
            }

            IQueryable<Admission> admissionsQuery = context.Admissions
                .Where(a => a.IsDischarged == false)
                .Include(p => p.PatientsVitals)
                .ThenInclude(pv => pv.Vitals)
                .Include(a => a.Patient)
                .ThenInclude(p => p.PatientAllergies)
                .ThenInclude(pa => pa.ActiveIngredient)
                .Include(a => a.Patient)
                .Include(p => p.Patient.PatientBooking.ApplicationUser)
                .Include(pc => pc.Patient.PatientConditions)
                .ThenInclude(pc => pc.ChronicCondition)
                .Include(a => a.Patient)
                .ThenInclude(p => p.PatientMedications)
                .ThenInclude(pm => pm.Medication)
                .ThenInclude(am => am.MedicationIngredient)
                .ThenInclude(lc => lc.ActiveIngredient)
                .Include(a => a.ApplicationUser)
                .Include(a => a.Bed)
                .Include(a => a.Ward);

            if (!string.IsNullOrEmpty(patientId))
            {
                admissionsQuery = admissionsQuery.Where(a => a.Patient.IdNumber == patientId);
            }
            else if (admissionId > 0)
            {
                admissionsQuery = admissionsQuery.Where(p => p.Id == admissionId);
            }

            var admissions = await admissionsQuery.ToListAsync();

            if (admissions == null || !admissions.Any())
            {
                return NotFound($"No admissions found for PatientId: {patientId}");
            }

            ViewBag.VitalNames = context.Vital
                .Select(v => new SelectListItem { Value = v.VitalId.ToString(), Text = v.VitalName })
                .ToList();

            return View(admissions);
        }




        public async Task<IActionResult> ViewPatientVitals(int? admissionId)
        {
            if (admissionId == null)
            {
                return BadRequest("Admission ID cannot be null.");
            }

            var patientVitals = await context.PatientsVitals
                .Include(pv => pv.Admission.Patient)
                .Include(pv => pv.Vitals)
                .Where(pv => pv.AdmissionId == admissionId)
                .ToListAsync();

            if (patientVitals == null || !patientVitals.Any())
            {
                return NotFound("No vitals found for the specified admission ID.");
            }

            return View(patientVitals);
        }

        public async Task<IActionResult> ViewPrescription(int admissionId)
        {
            if (admissionId == null)
            {
                return BadRequest("Admission cannot be found");
            }

            var prescription = await context.MedicationsGiven
                .Include(a => a.PrescriptionItem.Prescription.Admission.Ward)
                .Include(a => a.PrescriptionItem.Prescription.Admission.Bed)
                .Include(a => a.PrescriptionItem.Prescription.Admission.Patient)
                .Include(a => a.PrescriptionItem.Prescription.ApplicationUser)
                .Where(a => a.PrescriptionItem.Prescription.AdmissionId == admissionId)
                .ToListAsync();

            return View(prescription);
        }

        [HttpGet]
        public IActionResult Discharge(int admissionId)
        {
            var admission = context.Admissions
                           .Include(a => a.Bed)
                           .FirstOrDefault(a => a.Id == admissionId);

            if (admission == null || admission.IsDischarged)
            {
                return NotFound();
            }

            var viewModel = new DischargeViewModel
            {
                AdmissionId = admissionId,
                DischargeTime = DateTime.UtcNow,
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Discharge(DischargeViewModel viewModel)
        {
            var admission = context.Admissions
                           .Include(a => a.Bed)
                           .FirstOrDefault(a => a.Id == viewModel.AdmissionId);

            if (admission == null || admission.IsDischarged)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the logged-in user's ID
            var nurse = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (nurse == null)
            {
                return NotFound();
            }

            admission.IsDischarged = true;
            context.SaveChanges();

            var discharge = new Discharge
            {
                AdmissionId = viewModel.AdmissionId,
                DischargeNote = viewModel.DischargeNote,
                DischargeTime = viewModel.DischargeTime,
                NurseId = nurse.Id // Set the NurseId to the logged-in user's ID
            };

            context.Discharges.Add(discharge);
            context.SaveChanges();

            var bed = context.Beds.Find(admission.BedId);
            if (bed == null)
            {
                return NotFound();
            }

            bed.IsAvailable = true;
            context.SaveChanges(); // Save changes to update the bed's availability
            TempData["SuccessMessage"] = "Patient has been discharged successfully...";

            return RedirectToAction("NursePatients");
        }


    }
}
