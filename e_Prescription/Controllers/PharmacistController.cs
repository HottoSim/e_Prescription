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
            var prescriptions = _context.Prescriptions.ToList();
            return View(prescriptions);
        }

        //Return Medication
        [HttpGet]
        public IActionResult Medication()
        {
            var medication = _context.Medications.ToList();
            return View(medication);
        }

        //Add new Medication
        [HttpGet]
        public IActionResult AddMedication()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddMedication(Medication medication)
        {
            _context.Medications.Add(medication);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Successfully added...";
            return RedirectToAction("Medication");
        }

        //Return Stock Records

    }
}
