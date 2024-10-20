using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using e_Prescription.Data;
using e_Prescription.Migrations;
using e_Prescription.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using e_Prescription.Models.ViewModels;
using NuGet.Versioning;

namespace e_Prescription.Controllers
{
    //[Authorize(Roles = "Nurse")]
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
            if (user == null)
            {
                return NotFound();
            }

            var nurseName = $"{user.FirstName} {user.LastName}"; // Fetch full name

            

            // Fetch the number of patients admitted by the current nurse
            var admittedPatientsCount = context.Admissions
                .Count(a => a.NurseId == user.Id && !a.IsDischarged);


            // Fetch the number of patients booked for admission
            var bookedPatientsCount = context.BookingTreatments
                .Count(b => b.PatientBooking.Patient.Status == "Not Admitted");

            var model = new NurseLandingPageViewModel
            {
                NurseName = nurseName,
                AdmittedPatientsCount = admittedPatientsCount,
                BookedPatientsCount = bookedPatientsCount,

                AdmittedCount = context.Admissions.Where(a => !a.IsDischarged).Count(),
                DischargedCount = context.Admissions.Where(a => a.IsDischarged).Count(),
                BookedPatients = context.BookingTreatments.Where(p => p.PatientBooking.Patient.Status == "Not Admitted").Count()

            };

            return View(model);
        }

        // View bookings
        public IActionResult Index(string searchIdNumber, DateTime? searchDate, string sortOrder)
        {
            // Set default search date to today's date if not provided
            if (!searchDate.HasValue)
            {
                searchDate = DateTime.Today; // Set to today
            }

            var bookings = context.BookingTreatments.OrderBy(p => p.PatientBooking.Patient.Firstname)
                .Where(b => b.PatientBooking.Patient.Status == "Not Admitted")
                .Include(pb => pb.PatientBooking.Patient)
                .Include(t => t.PatientBooking.Theatre)
                .Include(t => t.TreatmentCode)
                .Include(pb => pb.PatientBooking.ApplicationUser)
                .AsQueryable();

            // Filter by searchIdNumber if provided
            if (!string.IsNullOrEmpty(searchIdNumber))
            {
                bookings = bookings.Where(b => b.PatientBooking.Patient.IdNumber == searchIdNumber);
            }

            // Filter by searchDate
            bookings = bookings.Where(b => b.PatientBooking.Date.Date == searchDate.Value.Date);

            // Sorting logic
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
            // Populate only the provinces for initial selection
            var provinces = context.Provinces.OrderBy(p => p.ProvinceName).ToList();
            ViewBag.Provinces = new SelectList(provinces, "ProvinceId", "ProvinceName");

            // Create empty dropdowns for cities and suburbs
            ViewBag.Cities = new SelectList(new List<City>(), "CityId", "CityName");
            ViewBag.Suburbs = new SelectList(new List<Suburb>(), "SuburbId", "SuburbName");

            // Find the patient and set patient details
            var patient = context.Patients.Find(patientId);
            if (patient == null)
            {
                return NotFound();
            }

            ViewBag.PatientName = $"{patient.Firstname} {patient.Lastname}";

            // Prepare a ViewModel with only patient contact info (not Province/City/Suburb)
            var contactDetail = context.ContactDetails.FirstOrDefault(c => c.PatientId == patientId);
            var viewModel = new ContactDetailViewModel
            {
                PatientId = patientId,
                CellphoneNumber = patient.CellphoneNumber,
                Email = patient.Email,
                ProvinceId = 0,   // Do not display ProvinceId initially
                CityId = 0,       // Do not display CityId initially
                SuburbId = 0,     // Do not display SuburbId initially
                StreetAddress = ""
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Contacts(ContactDetailViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                // Reload ViewBag data
                var provinces = context.Provinces.OrderBy(p => p.ProvinceName).ToList();
                ViewBag.Provinces = new SelectList(provinces, "ProvinceId", "ProvinceName");

                // Reload cities based on selected province
                var cities = context.Cities.Where(c => c.ProvinceId == viewModel.ProvinceId).ToList();
                ViewBag.Cities = new SelectList(cities, "CityId", "CityName");

                // Reload suburbs based on selected city
                var suburbs = context.Suburbs.Where(s => s.CityId == viewModel.CityId).ToList();
                ViewBag.Suburbs = new SelectList(suburbs, "SuburbId", "SuburbName");

                ViewBag.PatientName = context.Patients
                    .Where(p => p.PatientId == viewModel.PatientId)
                    .Select(p => $"{p.Firstname} {p.Lastname}")
                    .FirstOrDefault();

                return View(viewModel);
            }

            // Retrieve existing contact detail for the patient
            var existingContactDetail = context.ContactDetails.FirstOrDefault(c => c.PatientId == viewModel.PatientId);
            if (existingContactDetail != null)
            {
                // Update contact detail fields
                existingContactDetail.ProvinceId = viewModel.ProvinceId;
                existingContactDetail.CityId = viewModel.CityId;
                existingContactDetail.SuburbId = viewModel.SuburbId;
                existingContactDetail.StreetAddress = viewModel.StreetAddress;
            }
            else
            {
                // Add new contact detail if it doesn't exist
                context.ContactDetails.Add(new ContactDetail
                {
                    PatientId = viewModel.PatientId,
                    ProvinceId = viewModel.ProvinceId,
                    CityId = viewModel.CityId,
                    SuburbId = viewModel.SuburbId,
                    StreetAddress = viewModel.StreetAddress
                });
            }

            // Update patient contact information
            var patient = context.Patients.Find(viewModel.PatientId);
            if (patient != null)
            {
                patient.CellphoneNumber = viewModel.CellphoneNumber;
                patient.Email = viewModel.Email;
            }

            context.SaveChanges();
            return RedirectToAction("Admission", "Admission", new { patientId = viewModel.PatientId });
        }


        // Get City by the selected Province
        public JsonResult GetCityByProvinceId(int provinceId)
        {
            var city = context.Cities.Where(a => a.ProvinceId == provinceId).ToList();
            return Json(city);
        }

        // Get Suburb by the selected city
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
            ViewBag.VitalNames = context.Vital.OrderBy(v => v.VitalName)
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
                                // Split and parse the low and high blood pressure limits
                                var lowLimitParts = vitalInfo.LowLimit.Split('/');
                                double lowSystolic = double.Parse(lowLimitParts[0]);
                                double lowDiastolic = double.Parse(lowLimitParts[1]);

                                var highLimitParts = vitalInfo.HighLimit.Split('/');
                                double highSystolic = double.Parse(highLimitParts[0]);
                                double highDiastolic = double.Parse(highLimitParts[1]);

                                // Determine if systolic and diastolic are within their respective ranges
                                bool isSystolicLow = systolic < lowSystolic;
                                bool isSystolicHigh = systolic > highSystolic;
                                bool isDiastolicLow = diastolic < lowDiastolic;
                                bool isDiastolicHigh = diastolic > highDiastolic;

                                if (isSystolicLow || isDiastolicLow)
                                {
                                    // Either systolic or diastolic is below the defined low limit
                                    notifications.Add($"The reading for {vitalInfo.VitalName} is **LOWER** than normal range: {systolic}/{diastolic} {vitalInfo.Units} (Normal range: {vitalInfo.LowLimit} - {vitalInfo.HighLimit} {vitalInfo.Units})");
                                    vital.Note = "Lower than normal";
                                }
                                else if (isSystolicHigh || isDiastolicHigh)
                                {
                                    // Either systolic or diastolic is above the defined high limit
                                    notifications.Add($"The reading for {vitalInfo.VitalName} is **HIGHER** than normal range: {systolic}/{diastolic} {vitalInfo.Units} (Normal range: {vitalInfo.LowLimit} - {vitalInfo.HighLimit} {vitalInfo.Units})");
                                    vital.Note = "Higher than normal";
                                }
                                else
                                {
                                    // Both systolic and diastolic are within the normal range
                                    //notifications.Add($"The reading for {vitalInfo.VitalName} is **WITHIN NORMAL RANGE**: {systolic}/{diastolic} {vitalInfo.Units} (Normal range: {vitalInfo.LowLimit} - {vitalInfo.HighLimit} {vitalInfo.Units})");
                                    vital.Note = "Within normal range";
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
                                if (normalReading < lowLimit)
                                {
                                    notifications.Add($"The reading for {vitalInfo.VitalName} is **LOWER** than normal range: {normalReading} {vitalInfo.Units} (Normal range: {vitalInfo.LowLimit} - {vitalInfo.HighLimit} {vitalInfo.Units})");
                                    vital.Note = "Lower than normal";
                                }
                                else if(normalReading > highLimit)
                                {
                                    notifications.Add($"The reading for {vitalInfo.VitalName} is **HIGHER** than normal range: {normalReading} {vitalInfo.Units} (Normal range: {vitalInfo.LowLimit} - {vitalInfo.HighLimit} {vitalInfo.Units})");
                                    vital.Note = "Higher than normal";
                                }
                                else
                                {
                                    vital.Note = "Within normal range";
                                }
                            }
                            else
                            {
                                ViewBag.ErrorMessage = $"Invalid format for the reading of {vitalInfo.VitalName}. Please check the input!!";
                                return View(model);
                            }
                        }
                        else
                        {
                            ViewBag.ErrorMessage = $"Invalid format for the reading of {vitalInfo.VitalName}. Please check the input!!";
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

            ViewBag.VitalNames = context.Vital.OrderBy(v => v.VitalName)
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

            ViewBag.Wards = new SelectList(wards.OrderBy(w => w.WardName), "WardId", "WardName");
            ViewBag.Beds = new SelectList(beds.OrderBy(b => b.BedName), "BedId", "BedName");
            ViewBag.PatientName = $"{patient.Firstname} {patient.Lastname}";
            ViewBag.PatientId = patientId;
            ViewBag.Vitals = context.Vital.OrderBy(v => v.VitalName).ToList();
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
                        if (System.Text.RegularExpressions.Regex.IsMatch(patientVital.Reading, @"^\d{2,3}\/\d{2,3}$"))
                        {
                            var bloodPressureParts = patientVital.Reading.Split('/');
                            if (bloodPressureParts.Length == 2 &&
                                double.TryParse(bloodPressureParts[0], out double systolic) &&
                                double.TryParse(bloodPressureParts[1], out double diastolic))
                            {
                                // Parsing the low and high limits for systolic and diastolic readings
                                var lowLimitParts = vital.LowLimit.Split('/');
                                double lowSystolic = double.Parse(lowLimitParts[0]);
                                double lowDiastolic = double.Parse(lowLimitParts[1]);

                                var highLimitParts = vital.HighLimit.Split('/');
                                double highSystolic = double.Parse(highLimitParts[0]);
                                double highDiastolic = double.Parse(highLimitParts[1]);

                                // Check if systolic and diastolic readings are in the defined ranges
                                bool isSystolicLow = systolic < lowSystolic;
                                bool isSystolicHigh = systolic > highSystolic;
                                bool isDiastolicLow = diastolic < lowDiastolic;
                                bool isDiastolicHigh = diastolic > highDiastolic;

                                if (isSystolicLow || isDiastolicLow)
                                {
                                    // Systolic or Diastolic pressure is below the normal range
                                    notifications.Add($"The reading for {vital.VitalName} is **LOWER** than normal range: {systolic}/{diastolic} {vital.Units} (Normal range: {vital.LowLimit} - {vital.HighLimit} {vital.Units})");
                                    patientVital.Note = "Lower than normal";
                                }
                                else if (isSystolicHigh || isDiastolicHigh)
                                {
                                    // Systolic or Diastolic pressure is above the normal range
                                    notifications.Add($"The reading for {vital.VitalName} is **HIGHER** than normal range: {systolic}/{diastolic} {vital.Units} (Normal range: {vital.LowLimit} - {vital.HighLimit} {vital.Units})");
                                    patientVital.Note = "Higher than normal";
                                }
                                else
                                {
                                    // Systolic and Diastolic pressures are within the normal range
                                   // notifications.Add($"The reading for {vital.VitalName} is **WITHIN NORMAL RANGE**: {systolic}/{diastolic} {vital.Units} (Normal range: {vital.LowLimit} - {vital.HighLimit} {vital.Units})");
                                    patientVital.Note = "Within normal range";
                                }
                            }

                            else
                            {
                                ModelState.AddModelError(string.Empty, $"Invalid format for the blood pressure reading of {vital.VitalName}. Please check the input.");
                                ViewBag.Vitals = context.Vital.OrderBy(v => v.VitalName).ToList();
                                return View(admission);
                            }
                        }
                        else if (System.Text.RegularExpressions.Regex.IsMatch(patientVital.Reading, @"^\d+(\.\d+)?$"))
                        {
                            if (double.TryParse(vital.LowLimit, out double lowLimit) &&
                                double.TryParse(vital.HighLimit, out double highLimit) &&
                                double.TryParse(patientVital.Reading, out double normalReading))
                            {
                                if (normalReading < lowLimit)
                                {
                                    notifications.Add($"The reading for {vital.VitalName} is **LOWER** than normal range: {normalReading} {vital.Units} (Normal range: {vital.LowLimit} - {vital.HighLimit} {vital.Units})");
                                    patientVital.Note = "Lower than normal";
                                }
                                else if (normalReading > highLimit)
                                {
                                    notifications.Add($"The reading for {vital.VitalName} is **HIGHER** than normal range: {normalReading} {vital.Units} (Normal range: {vital.LowLimit} - {vital.HighLimit} {vital.Units})");
                                    patientVital.Note = "Higher than normal";
                                }
                                else
                                {
                                    patientVital.Note = "Within normal range";
                                }
                            }
                            else
                            {
                                ViewBag.ErrorMessage = $"Invalid format for {vital.VitalName}. Please check the input.";
                                ViewBag.Vitals = context.Vital.OrderBy(v => v.VitalName).ToList();
                                return View(admission);
                            }
                        }
                        else
                        {
                            ViewBag.ErrorMessage = $"Invalid format for the reading of {vital.VitalName}. Please check the input.";
                            ViewBag.Vitals = context.Vital.OrderBy(v => v.VitalName).ToList();

                            var wards = context.Wards.ToList();
                            var beds = new List<Bed>
                            {
                               new Bed()
                                {
                                BedId = 0, BedName = "--Select Bed--"
                               }
                            };

                            ViewBag.Wards = new SelectList(wards.OrderBy(w => w.WardName), "WardId", "WardName");
                            ViewBag.Beds = new SelectList(beds.OrderBy(b => b.BedName), "BedId", "BedName");
                            return View(admission);
                        }

                        context.PatientsVitals.Add(patientVital);
                    }
                }
                var heightInMeters = admission.Height / 100; // Convert height from cm to meters
                var bmiCalculation = (admission.Weight / (heightInMeters * heightInMeters));

                admission.BMI = bmiCalculation;
                context.Admissions.Add(admission);
                await context.SaveChangesAsync();

                if(admission.BMI > 24.0)
                {
                    notifications.Add($"BMI (Body Mass Index) is **HIGHER** than normal. Normal (18.5 - 18.5)");
                }
                else if(admission.BMI < 18.5)
                {
                    notifications.Add($"BMI (Body Mass Index) is **LOWER** than normal. Normal (18.5 - 18.5)");
                }

                var patient = context.Patients.Find(patientId);
                if (patient == null)
                {
                    return NotFound();
                }

                patient.Status = "Admitted";
                context.SaveChanges();

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
                    ViewBag.AlertMessage = "All patient Vitals seem normal!";
                }

                ViewBag.Vitals = context.Vital.OrderBy(v => v.VitalName).ToList();  // Reinitialize Vitals
                return View(admission);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error: Invalid format entered...";

                // Reinitialize Wards and Beds dropdowns
                var wards = context.Wards.ToList();
                var beds = context.Beds.Where(b => b.WardId == admission.WardId).ToList();
                beds.Insert(0, new Bed() { BedId = 0, BedName = "--Select Bed--" });

                ViewBag.Wards = new SelectList(wards.OrderBy(w => w.WardName), "WardId", "WardName");
                ViewBag.Beds = new SelectList(beds.OrderBy(b => b.BedName), "BedId", "BedName");
                ViewBag.Vitals = context.Vital.OrderBy(v => v.VitalName).ToList();  // Reinitialize Vitals
                ViewBag.PatientName = $"{context.Patients.Find(patientId).Firstname} {context.Patients.Find(patientId).Lastname}";
                ViewBag.PatientId = patientId;
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
            ViewBag.Allergies = new SelectList(context.ActiveIngredients.OrderBy(c => c.IngredientName), "ActiveIngredientId", "IngredientName");
            ViewBag.Conditions = new SelectList(context.ChronicConditions.OrderBy(c => c.Diagnosis), "ChronicCondotionId", "Diagnosis");
            ViewBag.Medications = new SelectList(context.Medications.OrderBy(c => c.MedicationName), "MedicationId", "MedicationName");

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult History(PatientHistory viewModel)
        {
            // Update patient allergies
            foreach (var allergyId in viewModel.SelectedAllergies)
            {
                // Check if the allergy already exists for the patient
                var existingAllergy = context.PatientAllergies
                    .FirstOrDefault(a => a.PatientId == viewModel.PatientId && a.ActiveIngredientId == allergyId);

                if (existingAllergy == null)
                {
                    // Add new allergy if it doesn't exist
                    context.PatientAllergies.Add(new PatientAllergies { PatientId = viewModel.PatientId, ActiveIngredientId = allergyId });
                }
            }

            // Update patient conditions
            foreach (var conditionId in viewModel.SelectedConditions)
            {
                // Check if the condition already exists for the patient
                var existingCondition = context.PatientConditions
                    .FirstOrDefault(c => c.PatientId == viewModel.PatientId && c.ChronicConditionId == conditionId);

                if (existingCondition == null)
                {
                    // Add new condition if it doesn't exist
                    context.PatientConditions.Add(new PatientCondition { PatientId = viewModel.PatientId, ChronicConditionId = conditionId });
                }
            }

            // Update patient medications
            foreach (var medicationId in viewModel.SelectedMedications)
            {
                // Check if the medication already exists for the patient
                var existingMedication = context.PatientMedications
                    .FirstOrDefault(m => m.PatientId == viewModel.PatientId && m.MedicationId == medicationId);

                if (existingMedication == null)
                {
                    // Add new medication if it doesn't exist
                    context.PatientMedications.Add(new PatientMedication { PatientId = viewModel.PatientId, MedicationId = medicationId });
                }
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
            ViewBag.Allergies = new SelectList(context.ActiveIngredients.OrderBy(c => c.IngredientName), "ActiveIngredientId", "IngredientName");
            ViewBag.Conditions = new SelectList(context.ChronicConditions.OrderBy(c => c.Diagnosis), "ChronicCondotionId", "Diagnosis");
            ViewBag.Medications = new SelectList(context.Medications.OrderBy(c => c.MedicationName), "MedicationId", "MedicationName");
            TempData["AlertMessage"] = $"{patient.Firstname} {patient.Lastname} has been admitted successfully!";

            return View(viewModel); // Return the updated view model
        }


        //Retrieve all admitted patients by the Nurse
        public async Task<IActionResult> NursePatients(string patientId, string sortOrder)
        {
            var user = await userManager.GetUserAsync(User);

            if(user == null) 
            {
                return NotFound();
            }

            // Query for admitted patients
            var admittedPatientsQuery = context.Admissions.OrderBy(a => a.Patient.Firstname)
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

            IQueryable<Admission> admissionsQuery = context.Admissions.OrderBy(a => a.Patient.Firstname)
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

        //View Prescription
        public async Task<IActionResult> ViewPrescription(int admissionId)
        {
            if (admissionId == 0)
            {
                return BadRequest("Admission cannot be found");
            }

            // Fetch Prescription Items
            var prescriptionItems = await context.PrescriptionItems
                .OrderBy(p => p.PharmacyMedication.MedicationName)
                .Include(pi => pi.Prescription.Admission.Ward)
                .Include(pi => pi.Prescription.Admission.Bed)
                .Include(pi => pi.Prescription.Admission.Patient)
                .Include(pi => pi.Prescription.ApplicationUser)
                .Include(pi => pi.PharmacyMedication.DosageForm)
                .Include(pi => pi.Prescription)
                .Where(pi => pi.Prescription.AdmissionId == admissionId && pi.Quantity > 0 && pi.Prescription.Status == "Dispensed")
                .ToListAsync();

            // Fetch Administered Medications
            var medicationsGiven = await context.MedicationsGiven
                .Include(mg => mg.PrescriptionItem.PharmacyMedication)
                .Include(mg => mg.PrescriptionItem.Prescription.Admission.Patient)
                .Include(nu => nu.ApplicationUser)
                .Where(mg => mg.PrescriptionItem.Prescription.AdmissionId == admissionId)
                .ToListAsync();

            // Create the ViewModel
            var viewModel = new PrescriptionViewModel
            {
                PrescriptionItems = prescriptionItems,
                MedicationsGiven = medicationsGiven
            };

            return View(viewModel);
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
            // Find the admission record
            var admission = await context.Admissions
                .Include(a => a.Bed)
                .Include(a => a.Patient) // Include the patient for status update
                .FirstOrDefaultAsync(a => a.Id == viewModel.AdmissionId);

            if (admission == null || admission.IsDischarged)
            {
                return NotFound();
            }

            // Get the logged-in nurse
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var nurse = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (nurse == null)
            {
                return NotFound();
            }

            // Update the admission status
            admission.IsDischarged = true;
            await context.SaveChangesAsync();

            // Create and save the discharge record
            var discharge = new Discharge
            {
                AdmissionId = viewModel.AdmissionId,
                DischargeNote = viewModel.DischargeNote,
                DischargeTime = viewModel.DischargeTime,
                NurseId = nurse.Id // Set the NurseId to the logged-in user's ID
            };

            context.Discharges.Add(discharge);
            await context.SaveChangesAsync();

            // Update the patient's status
            var patient = await context.Patients.FindAsync(admission.PatientId);
            if (patient == null)
            {
                return NotFound();
            }

            patient.Status = "Discharged";
            await context.SaveChangesAsync(); // Save changes to update the patient's status

            // Update the bed availability
            var bed = await context.Beds.FindAsync(admission.BedId);
            if (bed == null)
            {
                return NotFound();
            }

            bed.IsAvailable = true;
            await context.SaveChangesAsync(); // Save changes to update the bed's availability

            TempData["SuccessMessage"] = "Patient has been discharged successfully.";

            return RedirectToAction("NursePatients");
        }

        // GET: AdministerMedication
        // GET: AdministerMedication
        public async Task<IActionResult> AdministerMedication(int admissionId)
        {
            if (admissionId == 0)
            {
                return BadRequest("Admission cannot be found");
            }

            // Fetch prescription items related to the admission with a quantity greater than 0
            var prescriptionItems = await context.PrescriptionItems
                .Include(pi => pi.PharmacyMedication)
                .Where(pi => pi.Prescription.AdmissionId == admissionId && pi.Quantity > 0)
                .Select(pi => new
                {
                    pi.PrescriptionItemId,
                    MedicationName = pi.PharmacyMedication.MedicationName,
                    pi.Quantity
                })
                .ToListAsync();

            if (!prescriptionItems.Any())
            {
                return NotFound("No prescriptions found for the specified admission.");
            }

            ViewBag.PrescriptionItems = prescriptionItems;
            ViewBag.AdmissionId = admissionId;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdministerMedication(MedicationGiven medicationGiven)
        {
            if (medicationGiven == null || medicationGiven.PrescriptionItemId == 0)
            {
                ModelState.AddModelError("", "Invalid medication data.");
                return View(medicationGiven);
            }

            // Find the logged-in Nurse (current user)
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var nurse = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (nurse == null)
            {
                return NotFound();
            }

            // Find the corresponding prescription item
            var prescriptionItem = await context.PrescriptionItems
                .Include(pi => pi.Prescription)
                .FirstOrDefaultAsync(pi => pi.PrescriptionItemId == medicationGiven.PrescriptionItemId);

            if (prescriptionItem == null)
            {
                ModelState.AddModelError("", "Prescription item not found.");
                return View(prescriptionItem);
            }

            var setAdministerd = prescriptionItem.Quantity -= prescriptionItem.AdministeredQuantity;
            if (prescriptionItem.Quantity < medicationGiven.Quantity && medicationGiven.Quantity >= setAdministerd)
            {
                TempData["ErrorMessage"] = "Insufficient medication quantity.";
                return RedirectToAction("ViewPrescription", new { prescriptionItem.Prescription.AdmissionId });
            }

            prescriptionItem.AdministeredQuantity += medicationGiven.Quantity;

            // Set the NurseId and Time for MedicationGiven
            medicationGiven.NurseId = nurse.Id;
            medicationGiven.Time = DateTime.Now;

            // Update the status of the Prescription to "Administered"
           // prescriptionItem.Prescription.Status = "Administered";

            // Save the administered medication to the database
            context.MedicationsGiven.Add(medicationGiven);

            // Update the prescription item in the database(Status)
            context.PrescriptionItems.Update(prescriptionItem);

            // Update the prescription status in the database
            context.Prescriptions.Update(prescriptionItem.Prescription);

            await context.SaveChangesAsync();

            TempData["AdministrationSuccess"] = "Medication has been administered successfully.";

            // Redirect back to the ViewPrescription action with the admission ID
            var admissionId = prescriptionItem.Prescription?.AdmissionId ?? 0;
            if (admissionId == 0)
            {
                return RedirectToAction("Error", new { message = "Admission ID is invalid." });
            }

            return RedirectToAction("ViewPrescription", new { admissionId });
        }


        //Nurse Reports (Admission details)
        [HttpGet]
        public async Task<IActionResult> NurseReports(string patientId, DateTime? searchDate, string sortOrder)
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            // Query for admitted patients
            var PatientsQuery = context.Admissions.OrderBy(a => a.Patient.Firstname)
                .Where(a => a.NurseId == user.Id && a.IsDischarged)
                .Include(a => a.Patient)
                .ThenInclude(a => a.PatientBooking.ApplicationUser)
                .Include(a => a.ApplicationUser)
                .Include(a => a.Ward)
                .Include(a => a.Bed)
                .AsQueryable();  // Ensure queryable for further querying

            // Filter by patient ID if provided
            if (!string.IsNullOrEmpty(patientId))
            {
                PatientsQuery = PatientsQuery.Where(a => a.Patient.IdNumber.Contains(patientId));
            }

            if (searchDate.HasValue)
            {
                PatientsQuery = PatientsQuery.Where(b => b.AdmissionDate.Date.Date == searchDate.Value.Date);
            }
            // Sorting logic
            ViewData["FirstNameSortParam"] = sortOrder == "FirstName" ? "FirstName_desc" : "FirstName";
            switch (sortOrder)
            {
                case "FirstName_desc":
                    PatientsQuery = PatientsQuery.OrderByDescending(a => a.Patient.Firstname);
                    break;
                case "FirstName":
                    PatientsQuery = PatientsQuery.OrderBy(a => a.Patient.Firstname);
                    break;
                default:
                    PatientsQuery = PatientsQuery.OrderBy(a => a.Patient.IdNumber);
                    break;
            }

            var admittedPatients = await PatientsQuery.ToListAsync();

            return View(admittedPatients);
        }

    }
}

