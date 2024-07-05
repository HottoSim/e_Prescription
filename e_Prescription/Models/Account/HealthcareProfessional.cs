using e_Prescription.Data;
using System.ComponentModel.DataAnnotations;

namespace e_Prescription.Models.Account
{
    public class HealthcareProfessional
    {
        [Key]
        public int UserId { get; set; }

        
        [Required]
        [RegularExpression(@"^\d{7}$", ErrorMessage = "Invalid HPCSA number. It should be 7 digits long.")]
        public string HPCSANumber { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
