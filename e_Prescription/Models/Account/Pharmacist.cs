using e_Prescription.Data;
using e_Prescription.Models.Validations;
using System.ComponentModel.DataAnnotations;

namespace e_Prescription.Models.Account
{
    public class Pharmacist
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "*Required")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Invalid HPCSA Number Number")]
        public string PharmacyLicenseNumber { get; set; }

        [Required(ErrorMessage = "*Required")]
        [FutureDate(ErrorMessage = "The License Expiry Date must be a date in the future.")]
        public DateTime LicenseExpiryDate { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
