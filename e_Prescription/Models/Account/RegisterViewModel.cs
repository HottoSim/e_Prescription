using System.ComponentModel.DataAnnotations;

namespace e_Prescription.Models.Account
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^(\+27|0)[6-8][0-9]{8}$", ErrorMessage = "Invalid South African phone number.")]
        public string ContactNumber { get; set; }

        public string HPCSANumber { get; set; } // For healthcare professionals
    }
}
