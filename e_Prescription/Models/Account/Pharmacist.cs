using e_Prescription.Data;
using e_Prescription.Models.Validations;
using System.ComponentModel.DataAnnotations;

namespace e_Prescription.Models.Account
{
    public class Pharmacist
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [RegularExpression(@"^P\d{5}$", ErrorMessage = "Invalid Pharmacy License Number")]
        public string PharmacyLicenseNumber { get; set; }

        [Required]
        [FutureDate(ErrorMessage = "The License Expiry Date must be a date in the future.")]
        public DateTime LicenseExpiryDate { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
