using e_Prescription.Data;
using System.ComponentModel.DataAnnotations;
using e_Prescription.Models.Validations;

namespace e_Prescription.Models.Account
{
    public class Surgeon
    {
        [Key]
        public int UserId { get; set; }


        [Required(ErrorMessage = "*Required")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Invalid HPCSA Number Number")]
        public string HPCSANumber { get; set; }

        [Required(ErrorMessage = "*Required")]
        [FutureDate(ErrorMessage = "The License Expiry Date must be a date in the future.")]
        public DateTime LicenseExpiryDate { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string Specialization { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
