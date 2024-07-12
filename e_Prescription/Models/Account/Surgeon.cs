using e_Prescription.Data;
using System.ComponentModel.DataAnnotations;

namespace e_Prescription.Models.Account
{
    public class Surgeon
    {
        [Key]
        public int UserId { get; set; }

        
        [Required]
        [RegularExpression(@"^MP\d{7}$", ErrorMessage = "Invalid Medical License Number")]
        public string HPCSANumber { get; set; }

        [Required]
        public DateTime LicenseExpiryDate { get; set; }

        [Required]
        public string Specialization { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
