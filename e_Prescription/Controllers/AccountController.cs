using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using e_Prescription.Data;
using e_Prescription.Models.Account;
using System.Threading.Tasks;
using e_Prescription.Models;
using Microsoft.EntityFrameworkCore;

namespace e_Prescription.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var role = (await _userManager.GetRolesAsync(user))[0];

            var model = new UserViewModel
            {
                Lastname = user.LastName,
                Role = role,
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    if (!user.IsActive)
                    {
                        ModelState.AddModelError(string.Empty, "Your account is inactive. Please contact the administrator.");
                        return View(model);
                    }

                    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        var roles = await _userManager.GetRolesAsync(user);

                        if (roles.Contains("Nurse"))
                        {
                            return RedirectToAction("NurseLandingPage", "Admission");
                        }
                        if (roles.Contains("Admin"))
                        {
                            return RedirectToAction("Users", "Admin");
                        }
                        if (roles.Contains("Pharmacist"))
                        {
                            return RedirectToAction("Dashboard", "Pharmacist");
                        }

                        return RedirectToLocal(returnUrl);
                    }
                    if (result.IsLockedOut)
                    {
                        return View("Lockout");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult Register(string role)
        {
            var model = new RegisterViewModel { Role = role };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            ApplicationUser user;
            if (model.Role == "Admin")
            {
                user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    ContactNumber = model.ContactNumber,
                    Role = model.Role,
                    IsActive = true
                };

                var admin = new Admin
                {
                    ApplicationUser = user
                };
                _context.Admins.Add(admin);
            }
            else if (model.Role == "Surgeon")// For Healthcare Professionals
            {
                user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    ContactNumber = model.ContactNumber,
                    Role = model.Role,
                    IsActive = true
                };

                var surgeon = new Surgeon
                {
                    HPCSANumber = model.HPCSANumber,
                    Specialization = model.Specialization,
                    LicenseExpiryDate = model.LicenseExpiryDate,
                    ApplicationUser = user
                };

                _context.Surgeons.Add(surgeon);
            }
            else if(model.Role == "Pharmacist")
            {
                user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    ContactNumber = model.ContactNumber,
                    Role = model.Role,
                    IsActive = true
                };

                var pharmacist = new Pharmacist
                {
                    PharmacyLicenseNumber = model.PharmacyLicenseNumber,
                    LicenseExpiryDate = model.LicenseExpiryDate,
                    ApplicationUser = user
                };

                _context.Pharmacists.Add(pharmacist);
            }
            else
            {
                user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    ContactNumber = model.ContactNumber,
                    Role = model.Role,
                    IsActive = true
                };

                var nurse = new Nurse
                {
                    NursingLicenseNumber = model.NursingLicenseNumber,
                    LicenseExpiryDate = model.LicenseExpiryDate,
                    ApplicationUser = user
                };

                _context.Nurses.Add(nurse);
            }

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await EnsureRoleExistsAsync(model.Role);
                await _userManager.AddToRoleAsync(user, model.Role);
                TempData["SuccessMessage"] = $"{model.Role} has been registered successfully!";
                return RedirectToAction("Users", "Admin");
            }
            AddErrors(result);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        private async Task EnsureRoleExistsAsync(string role)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        // Update User Records (GET)
        [HttpGet]
        public async Task<IActionResult> UpdateUser(string userId, string role)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var model = new UpdateUsersViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ContactNumber = user.ContactNumber,
                Role = role,
                IsActive = user.IsActive
            };

            // Populate role-specific properties
            if (role == "Nurse")
            {
                var nurse = await _context.Nurses.Include(n => n.ApplicationUser).FirstOrDefaultAsync(n => n.ApplicationUser.Id == userId);
                if (nurse != null)
                {
                    model.LicenseExpiryDate = nurse.LicenseExpiryDate;
                }
            }

            return View(model); // Return view for updating the user
        }



        [HttpPost]
        public async Task<IActionResult> UpdateUser(UpdateUsersViewModel model)
        {

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }

            user.Email = model.Email;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.ContactNumber = model.ContactNumber;
            user.IsActive = true;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                if (model.Role == "Nurse")
                {
                    var nurse = await _context.Nurses.Include(n => n.ApplicationUser).FirstOrDefaultAsync(n => n.ApplicationUser.Id == model.UserId);
                    if (nurse != null)
                    {
                        nurse.LicenseExpiryDate = model.LicenseExpiryDate;
                        _context.Nurses.Update(nurse);
                    }
                }

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"{model.Role} has been updated successfully!";
                return RedirectToAction("Users", "Admin");
            }

            AddErrors(result);

            return View(model); // Return the main view on validation failure
        }
        // Deactivate users
        [HttpPost]
        public async Task<IActionResult> ToggleUserStatus(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            user.IsActive = false;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "User status updated successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error updating user status.";
            }

            return RedirectToAction("Users", "Admin"); // Adjust this as per your actual action/view name
        }

    }
}
