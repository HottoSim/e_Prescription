using e_Prescription.Data;
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
        public DateTime? LicenseExpiryDate { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
