using e_Prescription.Data;
using Microsoft.AspNetCore.Mvc;
using e_Prescription.Models.Account;
using e_Prescription.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Build.ObjectModelRemoting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using e_Prescription.Models.ViewModels;
//using e_Prescription.Migrations;



namespace e_Prescription.Controllers
{
    public class PharmacistController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration; // Add IConfiguration as a field
        private readonly UserManager<ApplicationUser> _userManager;

        public PharmacistController(ApplicationDbContext context, IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _configuration = configuration; // Assign IConfiguration
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UpdatePrescriptionStatus(int id)
        {
            var prescriptionItem = await _context.PrescriptionItems
                .Include(p => p.Prescription)
                    .ThenInclude(p => p.Admission)
                        .ThenInclude(a => a.Patient)
                .Include(p => p.Prescription)
                    .ThenInclude(p => p.ApplicationUser)
                .Include(p => p.PharmacyMedication)
                .FirstOrDefaultAsync(p => p.PrescriptionId == id);

            if (prescriptionItem == null)
            {
                return NotFound();
            }

            // Prepare a list of status options
            var statusOptions = new List<string> { "Pending","Rejected", "Dispensed" };

            var viewModel = new ViewPrescriptionViewModel
            {
                PrescriptionItem = prescriptionItem,
                StatusOptions = new SelectList(statusOptions)
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePrescriptionStatus(int id, string status, string rejectionReason)
        {
            if (string.IsNullOrEmpty(status))
            {
                ModelState.AddModelError(string.Empty, "Status is required.");
                var statusOptions = new List<string> { "Pending", "Rejected", "Dispensed" };

                // Fetch the prescription item again to repopulate the view model
                var prescriptionItem = await _context.PrescriptionItems
                .Include(p => p.Prescription)
                .ThenInclude(p => p.Admission)
                .ThenInclude(a => a.Patient)
                .Include(p => p.Prescription)
                .ThenInclude(p => p.ApplicationUser)
                .Include(p => p.PharmacyMedication)
                .FirstOrDefaultAsync(p => p.PrescriptionId == id);

                if (prescriptionItem == null)
                {
                    return NotFound();
                }

                var viewModel = new ViewPrescriptionViewModel
                {
                    PrescriptionItem = prescriptionItem,
                    StatusOptions = new SelectList(statusOptions)
                };

                return View(viewModel);
            }

            try
            {
                var prescriptionItemToUpdate = await _context.PrescriptionItems
                .Include(p => p.Prescription)
                .Include(p => p.PharmacyMedication) // Include PharmacyMedication to access medication detail
                .FirstOrDefaultAsync(p => p.PrescriptionId == id);

                if (prescriptionItemToUpdate == null)
                {
                    return NotFound();
                }

                // Update the status
                prescriptionItemToUpdate.Prescription.Status = status;

                if (status == "Rejected")
                {
                    // Handle rejection reason, e.g., log it or save it as needed
                    prescriptionItemToUpdate.RejectionNote = rejectionReason; // Adjust this line as needed
                }

                if (status == "Dispensed")
                {
                    // Reduce stock only if the status is "Dispensed"
                    var medication = prescriptionItemToUpdate.PharmacyMedication;
                    if (medication != null)
                    {
                        // Assuming you have a Quantity property in PrescriptionItem representing the amount dispensed
                        var quantityDispensed = prescriptionItemToUpdate.Quantity;
                        if (medication.QuantityOnHand >= quantityDispensed)
                        {
                            medication.QuantityOnHand -= quantityDispensed;
                        }
                        else
                        {
                            // Handle the case where there's not enough stock
                            ModelState.AddModelError(string.Empty, "Not enough stock to dispense the medication.");
                            return View(new ViewPrescriptionViewModel
                            {
                                PrescriptionItem = prescriptionItemToUpdate,
                                StatusOptions = new SelectList(new List<string> { "Pending", "Rejected", "Dispensed" })
                            });
                        }
                    }
                }

                // Save changes to the database
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Prescription status updated successfully.";
                return RedirectToAction("GetPrescriptions");
            }
            catch (Exception ex)
            {
                // Log the exception
                ModelState.AddModelError(string.Empty, "An error occurred while updating the prescription status: " + ex.Message);

                // Return the view with the current model state
                var statusOptions = new List<string> { "Pending", "Rejected", "Dispensed" };
                var prescriptionItem = await _context.PrescriptionItems
                .Include(p => p.Prescription)
                .ThenInclude(p => p.Admission)
                .ThenInclude(a => a.Patient)
                .Include(p => p.Prescription)
                .ThenInclude(p => p.ApplicationUser)
                .Include(p => p.PharmacyMedication)
                .FirstOrDefaultAsync(p => p.PrescriptionId == id);

                var viewModel = new ViewPrescriptionViewModel
                {
                    PrescriptionItem = prescriptionItem,
                    StatusOptions = new SelectList(statusOptions)
                };

                return View(viewModel);
            }
        }



        //Return Prescription
        [HttpGet]
        public IActionResult GetPrescriptions(bool showUrgentOnly = false)
        {
            
            var prescriptions = _context.PrescriptionItems
                .Include(p => p.Prescription.Admission.Patient)
                .Include(us => us.Prescription.ApplicationUser)
                .Include(p => p.PharmacyMedication)
                .AsQueryable();

            if (showUrgentOnly)
            {
                prescriptions = prescriptions.Where(p => p.Prescription.IsUrgent);
            }

            // Pass the showUrgentOnly value to the view
            ViewBag.ShowUrgentOnly = showUrgentOnly;

            return View(prescriptions.ToList());
        }

        // Search Action for Date Range
        [HttpGet]
        public IActionResult SearchPrescriptions(DateTime? startDate, DateTime? endDate)
        {
            // Check if both start and end dates are provided
            if (!startDate.HasValue || !endDate.HasValue)
            {
                TempData["ErrorMessage"] = "Please select both start and end dates.";
                return View(); // Redirect to the main page or wherever appropriate
            }

            // Redirect to the report generation with valid dates
            return RedirectToAction("GenerateDispensaryReport", new { startDate, endDate });
        }

        // Generating A Report
        [HttpGet]
        public async Task<IActionResult> GenerateDispensaryReport(DateTime? startDate, DateTime? endDate)
        {
            // Set default date range if no dates are provided
            // If no dates are provided, return an empty view
            if (!startDate.HasValue || !endDate.HasValue)
            {
                return View(new List<PDispensaryReportViewModel>());
            }

            // Get the current user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var pharmacistName = $"{user.FirstName} {user.LastName}";

            var start = startDate.Value;
            var end = endDate.Value.Date.AddDays(1).AddTicks(-1);

            // Fetch the prescriptions based on the date range
            var prescriptions = await _context.PrescriptionItems
                .Where(p => p.Prescription.Date >= start && p.Prescription.Date <= end)
                .Include(p => p.Prescription.Admission.Patient)
                .Include(p => p.PharmacyMedication)
                .ToListAsync();

            // Initialize view model
            var viewModel = new PDispensaryReportViewModel
            {
                PharmacistName = pharmacistName,
                StartDate = start,
                EndDate = end,
                Prescriptions = prescriptions ?? new List<PrescriptionItem>(), // Initialize to an empty list if null
                TotalDispensed = 0,
                TotalRejected = 0,
                SummaryPerMedication = new List<SummaryItem>() // Initialize to an empty list
            };

            // Check for empty prescriptions
            if (!viewModel.Prescriptions.Any())
            {
                TempData["ErrorMessage"] = "No prescriptions found for the selected date range.";
                return View(viewModel);
            }

            // Calculate total dispensed and rejected scripts
            viewModel.TotalDispensed = viewModel.Prescriptions.Count(p => p.Prescription.Status == "Dispensed");
            viewModel.TotalRejected = viewModel.Prescriptions.Count(p => p.Prescription.Status == "Rejected");

            // Summary per medication
            viewModel.SummaryPerMedication = viewModel.Prescriptions
                .Where(p => p.Prescription.Status == "Dispensed")
                .GroupBy(p => p.PharmacyMedication.MedicationName)
                .Select(g => new SummaryItem
                {
                    Medication = g.Key,
                    QuantityDispensed = g.Sum(p => p.Quantity)
                }).ToList();

            return View(viewModel);
        }


        //View patient history
        public async Task<IActionResult> ViewPatientHistory(string patientId, int admissionId)
        {
            if (string.IsNullOrEmpty(patientId) && admissionId == 0)
            {
                return View(new List<Admission>());
            }

            IQueryable<Admission> admissionsQuery = _context.Admissions.OrderBy(a => a.Patient.Firstname)
                .Where(a => a.IsDischarged == false)
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
                .Include(a => a.ApplicationUser);
                

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
          
            return View(admissions);
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
                .OrderBy(m => m.MedicationName)
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
                    })
                    .OrderBy(m => m.MedicationName) // Sort medications by MedicationName
                    .ToList(),

                StockOrders = _context.StockOrders
                    .Include(o => o.PharmacyMedication)
                    .Select(o => new StockOrderViewModel
                    {
                        StockOrderId = o.StockOrderId,
                        MedicationName = o.PharmacyMedication.MedicationName,
                        OrderQuantity = o.OrderQuantity,
                        Date = o.Date,
                        Status = o.Status
                    })
                    .OrderByDescending(o => o.Date) // Sort stock orders by Date in descending order (most recent first)
                    .ToList()
            };

            return View(viewModel);
        }

        //Delete button for ordered medication
        [HttpPost]
        public async Task<IActionResult> DeleteStockOrder(int stockOrderId)
        {
            var stockOrder = await _context.StockOrders.FindAsync(stockOrderId);
            if (stockOrder == null)
            {
                return NotFound();
            }

            _context.StockOrders.Remove(stockOrder);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Stock order deleted successfully.";
            return RedirectToAction("OrderMedications"); // Redirect back to the order page
        }


        [HttpPost]
        public async Task<IActionResult> OrderMedications(MedicationOrderViewModel model)
        {
            var emailService = new EmailService(_configuration);
            var purchaseManagerEmail = "nmostert@nmmu.ac.za"; // Replace with actual email

            var orderedMedications = new List<string>(); // To track medications being ordered

            foreach (var item in model.Medications.Where(m => m.IsSelected && m.OrderQuantity > 0))
            {
                // Fetch the medication name if not already in the view model
                var pharmacyMedication = await _context.PharmacyMedications
                    .FirstOrDefaultAsync(m => m.PharmacyMedicationId == item.PharmacyMedicationId);

                if (pharmacyMedication == null)
                {
                    continue; // Skip if medication not found
                }

                var stockOrder = new StockOrder
                {
                    PharmacyMedicationId = item.PharmacyMedicationId,
                    Date = DateTime.Now,
                    OrderQuantity = item.OrderQuantity,
                    Status = "Ordered"
                };

                _context.StockOrders.Add(stockOrder);

                // Use the fetched medication name (or ensure it's in the model)
                orderedMedications.Add($"{pharmacyMedication.MedicationName} - Quantity: {item.OrderQuantity}");
            }

            await _context.SaveChangesAsync();

            // If medications were ordered, send an email to the purchase manager
            if (orderedMedications.Any())
            {
                var subject = "Stock Order Notification";
                var message = $"Dear Purchase Manager,<br/><br/>The following medications have been ordered:<br/>" +
                              string.Join("<br/>", orderedMedications) +
                              "<br/><br/>Best regards,<br/>Pharmacy Team<br/></br/>";

                await emailService.SendEmailAsync(purchaseManagerEmail, subject, message);
            }

            TempData["EmailMessage"] = "Stock order placed successfully and email notification sent.";
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

        [HttpGet]
        public IActionResult EditReceivedMedication(int? id)
        {
            var stockReceived = _context.StockReceived
                .Include(o => o.StockOrder)
                .ThenInclude(o => o.PharmacyMedication)
                .FirstOrDefault(o => o.StockOrderId == id);

            if (stockReceived == null)
            {
                return NotFound();
            }

            var viewModel = new ReceivedOrderViewModel
            {
                StockOrderId = stockReceived.StockOrderId,
                MedicationName = stockReceived.StockOrder.PharmacyMedication.MedicationName,
                QuantityReceived = stockReceived.QuantityReceived,
                DateReceived = stockReceived.Date
            };

            return View(viewModel);
        }

        // POST: EditReceivedMedication 1        
        [HttpPost]
        public async Task<IActionResult> EditReceivedMedication(ReceivedOrderViewModel model)
        {
            // Find the stock received record
            var stockReceived = _context.StockReceived
                .Include(o => o.StockOrder)
                .FirstOrDefault(o => o.StockOrderId == model.StockOrderId);

            if (stockReceived == null)
            {
                return NotFound();
            }

            // Store the previous quantity before updating
            int previousQuantity = stockReceived.QuantityReceived;

            // Update the QuantityReceived and Date
            stockReceived.QuantityReceived = model.QuantityReceived;
            stockReceived.Date = DateTime.Now;

            // Update the related stock order and medication
            var medication = _context.PharmacyMedications
                .FirstOrDefault(m => m.PharmacyMedicationId == stockReceived.StockOrder.PharmacyMedicationId);

            if (medication != null)
            {
                // Adjust the medication quantity based on the old and new quantity received
                medication.QuantityOnHand -= previousQuantity; // Subtract the old quantity
                medication.QuantityOnHand += model.QuantityReceived; // Add the new quantity

                _context.PharmacyMedications.Update(medication);
            }

            _context.StockReceived.Update(stockReceived);
            await _context.SaveChangesAsync();

            // Redirect to the correct action or view
            return RedirectToAction("ReceivedMedicationRecords");
        }

    }
}
