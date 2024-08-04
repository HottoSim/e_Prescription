using e_Prescription.Models.Validations;
using System.ComponentModel.DataAnnotations;

namespace e_Prescription.Models.Account
{
    public class UpdateUsersViewModel
    {


        public string UserId { get; set; }
        [Required(ErrorMessage = "*Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "*Required")]

        public string LastName { get; set; }
        [Required(ErrorMessage = "*Required")]
        [RegularExpression(@"^(\+27|0)[6-8][0-9]{8}$", ErrorMessage = "Invalid South African phone number.")]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string Role { get; set; }

        [Required(ErrorMessage = "*Required")]
        public bool IsActive { get; set; }

        // Role-specific properties
        //public string HPCSANumber { get; set; }
        [Required(ErrorMessage = "*Required")]
        public string Specialization { get; set; }

        [Required(ErrorMessage = "*Required")]
        [FutureDate(ErrorMessage = "The License Expiry Date must be a date in the future.")]
        public DateTime LicenseExpiryDate { get; set; }
        //public string PharmacyLicenseNumber { get; set; }
        //public string NursingLicenseNumber { get; set; }
    }
}
