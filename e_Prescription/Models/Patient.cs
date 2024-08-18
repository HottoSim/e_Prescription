using e_Prescription.Models.Validations;
using System.ComponentModel.DataAnnotations;

namespace e_Prescription.Models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "*Required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "*Required")]                                                                                                                                                        
        [RegularExpression("^(\\d{2})(0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01])(\\d{4})([01])(\\d)(\\d)$", ErrorMessage = "Invalid South African ID number format.")]
        public string IdNumber { get; set; }

        [Required(ErrorMessage = "*Required")]
        [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "Date of Birth must be in the format YYYY-MM-DD.")]
        [NotInFuture(ErrorMessage = "Date of Birth cannot be in the future.")]
        public DateTime DateOfBirth { get; set; }


        // public string Surgeon { get; set; }

        public bool IsActive { get; set; } = false;

        public virtual List<PatientAllergies> PatientAllergies { get; set; } = new List<PatientAllergies>();

        public virtual List<PatientCondition> PatientConditions { get; set; } = new List<PatientCondition>();

        public virtual List<PatientMedication> PatientMedications { get; set; } = new List<PatientMedication>();

       
        public virtual PatientBooking PatientBooking { get; set; }

    }
}
