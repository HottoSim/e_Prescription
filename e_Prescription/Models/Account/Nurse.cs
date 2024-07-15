using e_Prescription.Data;
using e_Prescription.Models.Validations;
using System.ComponentModel.DataAnnotations;

namespace e_Prescription.Models.Account
{
    public class Nurse
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [RegularExpression(@"^\d{7}$", ErrorMessage = "Invalid Nursing License Number")]
        public string NursingLicenseNumber { get; set; }

        [Required]
        [FutureDate(ErrorMessage = "The License Expiry Date must be a date in the future.")]
        public DateTime LicenseExpiryDate { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
