using e_Prescription.Data;
using Microsoft.AspNetCore.Mvc;
using e_Prescription.Models.Account;
using e_Prescription.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Build.ObjectModelRemoting;

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
            return View();
        }
        //Return beds and wards
        public IActionResult ManageWards()
        {
            var beds = _context.Beds.Include(b => b.Ward).ToList();
            return View(beds);
        }
    }
}
