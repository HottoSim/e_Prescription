using e_Prescription.Data;
using Microsoft.AspNetCore.Mvc;
using e_Prescription.Models.Account;
using e_Prescription.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Build.ObjectModelRemoting;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace e_Prescription.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Users()
        {
            var ActiveNurseCount = _context.Users.Count(a => a.Role == "Nurse" && a.IsActive);
            var NonActiveNurseCount = _context.Users.Count(a => a.Role == "Nurse" && a.IsActive == false);

            var ActiveSurgeonCount = _context.Users.Count(a => a.Role == "Surgeon" && a.IsActive);
            var NonActiveSurgeonCount = _context.Users.Count(a => a.Role == "Surgeon" && a.IsActive == false);

            var ActivePharmacistCount = _context.Users.Count(a => a.Role == "Pharmacist" && a.IsActive);
            var NonActivePharmacistCount = _context.Users.Count(a => a.Role == "Pharmacist" && a.IsActive == false);

            var ActiveAdminCount = _context.Users.Count(a => a.Role == "Admin" && a.IsActive);
            var NonActiveAdminCount = _context.Users.Count(a => a.Role == "Admin" && a.IsActive == false);

            var model = new UserCountViewModel
            {
                NurseCount = ActiveNurseCount,
                NoNurseCount = NonActiveNurseCount,

                SurgeonCount = ActiveSurgeonCount,
                NoSurgeonCount = NonActiveSurgeonCount,

                PharmacistCount = ActivePharmacistCount,
                NoPharmacistCount = NonActivePharmacistCount,

                AdminCount = ActiveAdminCount,
                NoAdminCount = NonActiveAdminCount,
            };

            return View(model);
        }

        //View Users
        //View Nurses
        [HttpGet]
        public IActionResult GetNurses()
        {
            var getNurses = _context.Nurses.Include(n => n.ApplicationUser).ToList();
            return View(getNurses);
        }

        //View Surgeon
        [HttpGet]
        public IActionResult GetSurgeons()
        {
            var getSurgeons = _context.Surgeons.Include(n => n.ApplicationUser).ToList();
            return View(getSurgeons);
        }
        //View Pharmacists
        [HttpGet]
        public IActionResult GetPharmacists()
        {
            var getPharmacists = _context.Pharmacists.Include(n => n.ApplicationUser).ToList();
            return View(getPharmacists);
        }

        //View Admins
        [HttpGet]
        public IActionResult GetAdmins()
        {
            var getAdmins = _context.Admins.Include(n => n.ApplicationUser).ToList();
            return View(getAdmins);
        }






        //Returning vitals
        public IActionResult ManageVitals()
        {
            var vitalsList = _context.Vitals.ToList();
            return View(vitalsList);
        }

        //Add new vital
        [HttpGet]
        public IActionResult AddVitals()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddVitals(Vital vital)
        {
            _context.Vitals.Add(vital);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Successfully added...";
            return RedirectToAction("ManageVitals");
        }

        //Returns Beds and wards
        [HttpGet]
        public IActionResult ManageWards()
        {
            var beds = _context.Beds.Include(b => b.Ward).ToList();
            return View(beds);
        }

        //Add wards
        [HttpGet]
        public IActionResult AddWard()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddWard(Ward ward)
        {
            _context.Wards.Add(ward);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Ward has been successfully added...";
            return RedirectToAction("ManageWards");
        }


        [HttpGet]
        public IActionResult AddBed()
        {
            ViewBag.getWards = new SelectList(_context.Wards, "WardId", "WardName");
            return View();
        }

        [HttpPost]
        public IActionResult AddBed(Bed bed)
        {
            _context.Beds.Add(bed);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Successfully added...";
            ViewBag.getWards = new SelectList(_context.Wards, "WardId", "WardName");

            return RedirectToAction("ManageWards");

        }

        //Return Medications
        [HttpGet]
        public IActionResult ManageMedication()
        {
            var medication = _context.MedicationIngredients
                                                         .Include(m => m.Medication)
                                                         .Include( a => a.ActiveIngredient)
                                                         .ToList();
            return View(medication);
        }

        //Add Medication
        [HttpGet]
        public IActionResult AddMedication()
        {
            ViewBag.getDosage = new SelectList(_context.DosageForms, "DosageFormId", "Description");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddMedication(Medication medication)
        {
            _context.Medications.Add(medication);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Medication has been successfully added...";
            ViewBag.getDosage = new SelectList(_context.DosageForms, "DosageFormId", "Description");

            return RedirectToAction("ManageMedication");
        }

        [HttpGet]
        public IActionResult AddMedicationIngredient()
        {
            // Initialize ViewBag with SelectList for dropdowns
            ViewBag.getMedication = new SelectList(_context.Medications, "MedicationId", "MedicationName");
            ViewBag.getIngredient = new SelectList(_context.ActiveIngredients, "ActiveIngredientId", "IngredientName");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddMedicationIngredient(MedicationIngredient medicationIngredient)
        {

            _context.MedicationIngredients.Add(medicationIngredient);
            await _context.SaveChangesAsync();
            // If ModelState is not valid, repopulate the dropdown lists
            ViewBag.getMedication = new SelectList(_context.Medications, "MedicationId", "MedicationName");
            ViewBag.getIngredient = new SelectList(_context.ActiveIngredients, "ActiveIngredientId", "IngredientName");

            TempData["SuccessMessage"] = "Medication ingredients has been successfully added...";

            // Redirect to the list view (Index) after successful addition
            return RedirectToAction("ManageMedication");

        }

        //Return Chronic conditions 
        [HttpGet]
        public IActionResult ManageChronicConditions()
        {
            var conditions = _context.ChronicConditions.ToList();
            return View(conditions);
        }

        //Add new conditions
        [HttpGet]
        public IActionResult AddChronicCondtion()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddChronicCondtion(ChronicCondition condition)
        {
            _context.ChronicConditions.Add(condition);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Medication ingredients has been successfully added...";

            return RedirectToAction("ManageChronicConditions");

        }

        //Return Active Ingredients
        [HttpGet]
        public IActionResult ManageActiveIngredient()
        {
            var active = _context.ActiveIngredients.ToList();
            return View(active);
        }
        //Add new Active Ingredient
        [HttpGet]
        public IActionResult AddActiveIngredient()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddActiveIngredient(ActiveIngredient activeIngredient)
        {
            _context.ActiveIngredients.Add(activeIngredient);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Ingredient has been added...";
            return View();
        }


        //Return Treatments codes
        [HttpGet]
        public IActionResult ManageTreatmentCodes()
        {
            var codes = _context.TreatmentCodes.ToList();
            return View(codes);
        }

        //Add New Codes
        [HttpGet]
        public IActionResult AddTreatmentCode()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddTreatmentCode(TreatmentCode code)
        {
            _context.TreatmentCodes.Add(code);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Successfully added...";
            return RedirectToAction("ManageTreatmentCodes");
        }

        //Return Theatre
        [HttpGet]
        public IActionResult ManageTheatre()
        {
            var theatre = _context.Theatre.ToList();
            return View(theatre);
        }

        //Add new Theatre
        [HttpGet]
        public IActionResult AddTheatre()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddTheatre(Theatre theatre)
        {
            _context.Theatre.Add(theatre);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Successfully added...";
            return RedirectToAction("ManageTheatre");
        }
    }
}
