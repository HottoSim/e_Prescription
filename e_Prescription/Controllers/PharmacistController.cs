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
        public IActionResult Dashboard()
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

        //Update Medication List
        public IActionResult UpdateMedication(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var objList = _context.PharmacyMedications.Find(id);
            if (objList == null)
            {
                return NotFound();
            }

            ViewBag.DosageForms = new SelectList(_context.DosageForms, "DosageFormId", "Description");
            ViewBag.ActiveIngredients = new SelectList(_context.ActiveIngredients, "ActiveIngredientId", "IngredientName");
            return View(objList);
        }
        //Return Updated medication list
        [HttpPost]
        public IActionResult UpdateMedication(PharmacyMedication medication)
        {
           _context.PharmacyMedications.Update(medication);
           ViewBag.DosageForms = new SelectList(_context.DosageForms, "DosageFormId", "Description", medication.DosageFormId);
           ViewBag.ActiveIngredients = new SelectList(_context.ActiveIngredients, "ActiveIngredientId", "IngredientName");
           _context.SaveChanges();
            return RedirectToAction("MedicationList");
        }

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


        //Stock management
        // Action to display the order form
        public IActionResult OrderMedications()
        {
            var viewModel = new MedicationOrderViewModel
            {
                Medications = _context.PharmacyMedications
                    .Select(m => new PharmacyMedicationViewModel
                    {
                        PharmacyMedicationId = m.PharmacyMedicationId,
                        MedicationName = m.MedicationName,
                        QuantityOnHand = m.QuantityOnHand,
                        ReorderLevel = m.ReorderLevel
                    }).ToList(),

                StockOrders = _context.StockOrders
                    .Include(o => o.PharmacyMedication)
                    .Select(o => new StockOrderViewModel
                    {
                        StockOrderId = o.StockOrderId,
                        MedicationName = o.PharmacyMedication.MedicationName,
                        OrderQuantity = o.OrderQuantity,
                        Date = o.Date,
                        Status = o.Status
                    }).ToList()
            };

            return View(viewModel);
        }

        // Action to process the order
        [HttpPost]
        public IActionResult OrderMedications(MedicationOrderViewModel model)
        {
            foreach (var item in model.Medications.Where(m => m.IsSelected && m.OrderQuantity > 0))
            {
                var stockOrder = new StockOrder
                {
                    PharmacyMedicationId = item.PharmacyMedicationId,
                    Date = DateTime.Now,
                    OrderQuantity = item.OrderQuantity,
                    Status = "Ordered"
                };

                _context.StockOrders.Add(stockOrder);
            }

            _context.SaveChanges();

            return RedirectToAction("OrderMedications");
        }

        // Action to return selected orders
        [HttpGet]
        public IActionResult ReturnOrders()
        {
            var viewModel = new MedicationOrderViewModel
            {
                Medications = new List<PharmacyMedicationViewModel>(), // Not needed for this action

                StockOrders = _context.StockOrders
                    .Include(o => o.PharmacyMedication)
                    .Select(o => new StockOrderViewModel
                    {
                        StockOrderId = o.StockOrderId,
                        MedicationName = o.PharmacyMedication.MedicationName,
                        OrderQuantity = o.OrderQuantity,
                        Date = o.Date,
                        Status = o.Status
                    }).ToList()
            };

            return View(viewModel);
        }

        // GET: ReceivedMedication
        [HttpGet]
        public IActionResult ReceivedMedication()
        {
            var viewModel = new ReceivedOrderViewModel
            {
                Medications = _context.StockOrders
                    .Include(o => o.PharmacyMedication)
                    .Select(o => new SelectListItem
                    {
                        Value = o.PharmacyMedicationId.ToString(),
                        Text = o.PharmacyMedication.MedicationName
                    })
                    .Distinct()
                    .ToList()
            };

            return View(viewModel);
        }

        // POST: ReceivedMedication
        // POST: ReceivedMedication
        [HttpPost]
        public async Task<IActionResult> ReceivedMedication(ReceivedOrderViewModel model)
        {
            var medication = _context.PharmacyMedications
                .SingleOrDefault(m => m.PharmacyMedicationId == model.SelectedMedicationId);

            if (medication != null)
            {
                // Find the related StockOrder
                var stockOrder = _context.StockOrders
                    .FirstOrDefault(o => o.PharmacyMedicationId == model.SelectedMedicationId);

                if (stockOrder != null)
                {
                    // Create a new StockReceived for the received medication
                    var stockReceived = new StockReceived
                    {
                        StockOrderId = stockOrder.StockOrderId,
                        Date = DateTime.Now,
                        QuantityReceived = model.QuantityReceived
                    };

                    // Set the status of the related StockOrder
                    stockOrder.Status = "Received";
                    _context.StockOrders.Update(stockOrder);

                    // Add the new StockReceived entry to the database
                    _context.StockReceived.Add(stockReceived);

                    // Update the medication quantity
                    medication.QuantityOnHand += model.QuantityReceived;
                    _context.PharmacyMedications.Update(medication);

                    await _context.SaveChangesAsync();

                    // Redirect to a success page or back to the form
                    return RedirectToAction("ReceivedMedicationRecords");
                }
                else
                {
                    ModelState.AddModelError("", "Stock order not found.");
                }
            }
            else
            {
                ModelState.AddModelError("", "Selected medication is not found.");
            }

            // If model state is invalid or medication/stock order not found, redisplay the form
            model.Medications = _context.StockOrders
                .Include(o => o.PharmacyMedication)
                .Select(o => new SelectListItem
                {
                    Value = o.PharmacyMedicationId.ToString(),
                    Text = o.PharmacyMedication.MedicationName
                })
                .Distinct()
                .ToList();

            return View(model);
        }


        // GET: ReceivedMedication/Records
        public async Task<IActionResult> ReceivedMedicationRecords()
        {
            var viewModel = await _context.StockReceived
                .Include(o => o.StockOrder)
                .Select(o => new ReceivedOrderViewModel
                {
                    StockOrderId = o.StockOrderId,
                    MedicationName = o.StockOrder.PharmacyMedication.MedicationName,
                    QuantityReceived = o.QuantityReceived,
                    DateReceived = o.Date,
                    Status = o.StockOrder.Status
                })
                .ToListAsync();

            return View(viewModel);
        }

        ////Update Stock Received
        //public IActionResult UpdateStockReceived(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    var objList = _context.StockOrders.Find(id);
        //    if (objList == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(objList);
        //}
        ////Return Updated medication list
        //[HttpPost]
        //public IActionResult UpdateStockReceived(ReceivedOrderViewModel model)
        //{
        //    _context.StockOrders.Update(stockReceived);
        //    _context.SaveChanges();
        //    return RedirectToAction("ReceivedMedicationRecords");
        //}

        ////Update Stock Received 2
        //public IActionResult UpdateStockReceived(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var stockOrder = _context.StockOrders
        //        .Include(o => o.PharmacyMedication)  // Ensure related data is included if needed
        //        .FirstOrDefault(o => o.StockOrderId == id);

        //    if (stockOrder == null)
        //    {
        //        return NotFound();
        //    }

        //    // Create a view model if necessary to match your form
        //    //var model = new ReceivedOrderViewModel
        //    //{
        //    //    StockOrderId = stockOrder.StockOrderId,
        //    //    SelectedMedicationId = stockOrder.PharmacyMedicationId,
        //    //    // Other properties as needed
        //    //};
        //    var model = new ReceivedOrderViewModel
        //    {
        //        Medications = _context.StockOrders
        //            .Include(o => o.PharmacyMedication)
        //            .Select(o => new SelectListItem
        //            {
        //                Value = o.PharmacyMedicationId.ToString(),
        //                Text = o.PharmacyMedication.MedicationName
        //            })
        //            .Distinct()
        //            .ToList()
        //    };

        //    return View(model);
        //}
        //[HttpPost]
        //public async Task<IActionResult> UpdateStockReceived(ReceivedOrderViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        // Repopulate dropdowns or other form elements if needed
        //        model.Medications = _context.StockOrders
        //            .Include(o => o.PharmacyMedication)
        //            .Select(o => new SelectListItem
        //            {
        //                Value = o.PharmacyMedicationId.ToString(),
        //                Text = o.PharmacyMedication.MedicationName
        //            })
        //            .Distinct()
        //            .ToList();
        //        return View(model);
        //    }

        //    var stockOrder = await _context.StockOrders.FindAsync(model.StockOrderId);
        //    if (stockOrder == null)
        //    {
        //        return NotFound();
        //    }

        //    // Update the stock order properties
        //    stockOrder.Status = "Updated"; // Set to the appropriate status
        //                                   // Update other properties as needed

        //    // Save changes
        //    _context.StockOrders.Update(stockOrder);
        //    await _context.SaveChangesAsync();

        //    return RedirectToAction("ReceivedMedicationRecords");
        //}

    }
}
