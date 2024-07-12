using e_Prescription.Data;
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
        public DateTime LicenseExpiryDate { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
