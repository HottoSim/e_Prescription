﻿using e_Prescription.Data;
using Microsoft.AspNetCore.Mvc;
using e_Prescription.Models.Account;
using e_Prescription.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Build.ObjectModelRemoting;
using Microsoft.AspNetCore.Mvc.Rendering;
using  e_Prescription.Models.ViewModels;

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
            
            var vitalsList = _context.Vital.ToList();
            return View(vitalsList);
        }

        //Add new vital
        [HttpGet]
        public IActionResult AddVitals()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddVitals(Vitals vital)
        {
            _context.Vital.Add(vital);
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
            var medicationIngredients = _context.MedicationIngredients
                .Include(mi => mi.Medication)
                .Include(mi => mi.ActiveIngredient)
                .Include(mi => mi.Medication.DosageForm)
                .OrderBy(mi => mi.Medication.MedicationName)
                .ThenBy(mi => mi.ActiveIngredient.IngredientName)
                .ThenBy(mi => mi.IngredientStrength)
                .ToList();

            return View(medicationIngredients);
        }


        //Dosage forms
        [HttpGet]
        public IActionResult AddDosageForm()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddDosageForm(DosageForm dosage)
        {
            _context.DosageForms.Add(dosage);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Dosage form has been successfuly added...";

            return RedirectToAction("ManageMedication");
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
            return RedirectToAction("ManageActiveIngredient");
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

        // Manage location 
        [HttpGet]
        public IActionResult AddProvince()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddProvince(Province province)
        {
            _context.Provinces.Add(province);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Province has been successfully added...";
            return RedirectToAction("LocationManagement");
        }

        [HttpGet]
        public IActionResult AddCity()
        {
            ViewBag.getProvince = new SelectList(_context.Provinces, "ProvinceId", "ProvinceName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCity(City city)
        {
            _context.Cities.Add(city);
            await _context.SaveChangesAsync();
            ViewBag.getProvince = new SelectList(_context.Provinces, "ProvinceId", "ProvinceName");
            TempData["SuccessMessage"] = "City has been successfully added...";
            return RedirectToAction("LocationManagement");
        }

        [HttpGet]
        public IActionResult AddSuburb()
        {
            // Fetch all provinces for the Province dropdown
            ViewBag.getProvince = new SelectList(_context.Provinces, "ProvinceId", "ProvinceName");

            // Initially, the City dropdown will be empty
            ViewBag.getCity = new SelectList(Enumerable.Empty<SelectListItem>());

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSuburb(Suburb suburb)
        {

            _context.Suburbs.Add(suburb);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Suburb has been successfully added...";

            // If model state is invalid, reload the form with the selections
            ViewBag.getProvince = new SelectList(_context.Provinces, "ProvinceId", "ProvinceName");
            ViewBag.getCity = new SelectList(_context.Cities.Where(c => c.ProvinceId == suburb.ProvinceId), "CityId", "CityName");

            return RedirectToAction("LocationManagement");

        }

        [HttpGet]
        public JsonResult GetCitiesByProvince(int provinceId)
        {
            var cities = _context.Cities
                .Where(c => c.ProvinceId == provinceId)
                .Select(c => new { c.CityId, c.CityName })
                .ToList();

            return Json(cities);
        }


        //Return locations 
        public IActionResult LocationManagement()
        {
            var provinces = _context.Provinces
                .Select(p => new ProvinceViewModel
                {
                    ProvinceId = p.ProvinceId,
                    ProvinceName = p.ProvinceName,
                    Cities = p.Cities.Select(c => new CityViewModel
                    {
                        CityId = c.CityId,
                        CityName = c.CityName,
                        Suburbs = c.Suburbs.Select(s => new SuburbViewModel
                        {
                            SuburbId = s.SuburbId,
                            SuburbName = s.SuburbName,
                            PostalCode = s.PostalCode
                        }).ToList()
                    }).ToList()
                }).ToList();

            return View(provinces);
        }

        //Manage Contra indications
        public IActionResult ManageContraIndications()
        {
            var indications = _context.ContraIndications
                .Include(cm => cm.ChronicCondition)
                .Include(cm => cm.ActiveIngredient)
                .ToList();
            return View(indications);
        }

        //Add Contra indications
        //
        [HttpGet]
        public IActionResult AddContraIndication()
        {
            ViewBag.getCondtions = new SelectList(_context.ChronicConditions, "ChronicCondotionId", "Diagnosis");
            ViewBag.getIngredients = new SelectList(_context.ActiveIngredients, "ActiveIngredientId", "IngredientName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddContraIndication(ContraIndication contraIndication)
        {
            _context.ContraIndications.Add(contraIndication);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Indication has been successfully added...";

            ViewBag.getCondtions = new SelectList(_context.ChronicConditions, "ChronicCondotionId", "Diagnosis");
            ViewBag.getIngredients = new SelectList(_context.ActiveIngredients, "ActiveIngredientId", "IngredientName");
            return RedirectToAction("ManageContraIndications");
        }

        //Manage Medical interactions 
        [HttpPost]
        public IActionResult ManageMedicalInteractions()
        {
            var medication = _context.MedicationInteractions.ToList();
            return View(medication);
        }

        //Add medication interaction
        [HttpGet]
        public IActionResult AddMedicalInteraction()
        {
            ViewBag.getMedication = new SelectList(_context.Medications, "MedicationId", "MedicationName");
            ViewBag.getMedPharmacy = new SelectList(_context.PharmacyMedications, "MedicationId", "MedicationName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddMedicalInteraction(MedicationInteraction interaction)
        {
            _context.MedicationInteractions.Add(interaction);
            await _context.SaveChangesAsync();

            TempData["SucessInteraction"] = "Interation has been successfully added...";

            ViewBag.getMedication = new SelectList(_context.Medications, "MedicationId", "MedicationName");
            ViewBag.getMedPharmacy = new SelectList(_context.PharmacyMedications, "MedicationId", "MedicationName");

            return RedirectToAction("ManageMedicalInteractions");
        }
    }
}

