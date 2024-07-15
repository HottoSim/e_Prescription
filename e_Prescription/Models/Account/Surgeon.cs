using e_Prescription.Data;
using System.ComponentModel.DataAnnotations;
using e_Prescription.Models.Validations;

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
        [FutureDate(ErrorMessage = "The License Expiry Date must be a date in the future.")]
        public DateTime LicenseExpiryDate { get; set; }

        [Required]
        public string Specialization { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
