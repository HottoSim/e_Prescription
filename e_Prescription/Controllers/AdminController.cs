using e_Prescription.Data;
using Microsoft.AspNetCore.Mvc;
using e_Prescription.Models.Account;

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
    }
}
