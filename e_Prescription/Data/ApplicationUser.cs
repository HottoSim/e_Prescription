using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace e_Prescription.Data
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "*Required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^(\+27|0)[6-8][0-9]{8}$", ErrorMessage = "Invalid South African phone number.")]
        public string ContactNumber { get; set; }

        [Required]
        public string Role { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
