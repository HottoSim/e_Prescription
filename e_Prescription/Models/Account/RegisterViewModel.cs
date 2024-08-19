using System.ComponentModel.DataAnnotations;

namespace e_Prescription.Models.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "*Required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "*Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "*Required")]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Role { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "*Required")]
        [RegularExpression(@"^(\+27|0)[6-8][0-9]{8}$", ErrorMessage = "Invalid South African phone number.")]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "*Required")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Invalid HPCSA Number Number")]
        public string HPCSANumber { get; set; } // For healthcare professionals

        [Required(ErrorMessage = "*Required")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Invalid HPCSA Number Number")]
        public string PharmacyLicenseNumber { get; set; }

        [Required(ErrorMessage = "*Required")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Invalid HPCSA Number Number")]
        public string NursingLicenseNumber { get; set; }

        [Required(ErrorMessage = "*Required")]
        public DateTime LicenseExpiryDate { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string Specialization { get; set; }
    }
}
