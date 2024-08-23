using e_Prescription.Data;
using Microsoft.AspNetCore.Mvc;
using e_Prescription.Models.Account;
using e_Prescription.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Build.ObjectModelRemoting;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace e_Prescription.Controllers
{
    public class PharmacistController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PharmacistController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        //Return Prescription
        [HttpGet]
        public IActionResult GetPrescriptions()
        {
            var prescriptions = _context.Prescriptions.Where(a => a.Status == "Prescribed").ToList();
            return View(prescriptions);
        }

        //Return Medication
        [HttpGet]
        public IActionResult Medication()
        {
            var medication = _context.PharmacyMedications.OrderBy(m => m.MedicationName).ToList();
            return View(medication);
        }

        //Add new Medication
        [HttpGet]
        public IActionResult AddMedication()
        {
            ViewBag.ActiveIngredients = new SelectList(_context.ActiveIngredients.OrderBy(a => a.IngredientName), "ActiveIngredientId", "IngredientName");

            ViewBag.DosageForms = new SelectList(_context.DosageForms.OrderBy(d => d.Description), "DosageFormId", "Description");

            return View();
        }

        // POST: Add Medication
        [HttpPost]
        public async Task<IActionResult> AddMedication(PharmacyMedication medication, List<int> selectedIngredients, List<string> ingredientStrengths)
        {
           
                // Add the medication first
                _context.PharmacyMedications.Add(medication);
                await _context.SaveChangesAsync();

                // Now add the ingredients with the saved PharmacyMedicationId
                if (selectedIngredients != null && ingredientStrengths != null)
                {
                    for (int i = 0; i < selectedIngredients.Count; i++)
                    {
                        var ingredient = new PharmacyMedicationIngredient
                        {
                            PharmacyMedicationId = medication.PharmacyMedicationId, 
                            ActiveIngredientId = selectedIngredients[i],
                            Strength = ingredientStrengths[i]
                        };
                        _context.PharmacyMedicationIngredients.Add(ingredient);
                    }
                    await _context.SaveChangesAsync();
                }

            ViewBag.DosageForms = new SelectList(_context.DosageForms, "DosageFormId", "Description", medication.DosageFormId);
            ViewBag.ActiveIngredients = new SelectList(_context.ActiveIngredients, "ActiveIngredientId", "IngredientName");

            return RedirectToAction("MedicationList");
            
        }


        //Add Medication ingredients
        [HttpGet]
        public IActionResult AddIngredient(int medicationId)
        {
            var medication = _context.PharmacyMedications.Find(medicationId);

            if (medication == null)
            {
                return NotFound();
            }
            var model = new PharmacyMedicationIngredient
            {
                PharmacyMedicationId = medicationId,
            };

            ViewBag.MedicationId = medicationId; // Pass the PharmacyMedicationId to the view
            ViewBag.Ingredients = new SelectList(_context.ActiveIngredients.OrderBy(i => i.IngredientName), "ActiveIngredientId", "IngredientName");
            return View();
        }

        [HttpPost]
        public IActionResult AddIngredient(PharmacyMedicationIngredient model)
        {
            var medication = _context.PharmacyMedications.Find(model.PharmacyMedicationId);

            if (medication == null)
            {
                return NotFound();
            }

            var medic = new PharmacyMedicationIngredient
            {
                PharmacyMedicationId = model.PharmacyMedicationId,
                ActiveIngredientId = model.ActiveIngredientId,
                Strength = model.Strength
            };

            _context.PharmacyMedicationIngredients.Add(model);
            _context.SaveChanges();

            ViewBag.Message = "Ingredient has been added...";
            ViewBag.Ingredients = new SelectList(_context.ActiveIngredients, "ActiveIngredientId", "IngredientName");

            // Redirect back to the same AddIngredient action with the medicationId
            return RedirectToAction("AddIngredient", new { medicationId = model.PharmacyMedicationId });
        }


        //Stock management
        [HttpGet]
        public IActionResult ManageStock(int medicationId)
        {
            var medication = _context.PharmacyMedications.Find(medicationId);
            if (medication == null)
            {
                return NotFound();
            }
            return View(medication);
        }

        [HttpPost]
        public IActionResult ManageStock(int medicationId, int orderQuantity)
        {
            var medication = _context.PharmacyMedications.Find(medicationId);
            if (medication != null)
            {
                medication.QuantityOnHand += orderQuantity;
                _context.SaveChanges();
            }
            return RedirectToAction("MedicationList");
        }

        //Medication list
        [HttpGet]
        public IActionResult MedicationList()
        {
            var medications = _context.PharmacyMedications
                .Include(m => m.PharmacyMedicationIngredients)
                .ThenInclude(mi => mi.ActiveIngredient)
                .Include(m => m.DosageForm)
                .ToList();

            return View(medications);
        }



        //Return Stock Records
        [HttpGet]
        public IActionResult StockRecords()
        {
            var stockRecords = _context.PharmacyMedications.ToList();
            return View(stockRecords);
        }

        //Return Stock Ordered
        [HttpGet]
        public IActionResult StockOrdered()
        {
            var stockOrdered = _context.StockOrders.ToList();
            return View(stockOrdered);
        }
        

    }
}
