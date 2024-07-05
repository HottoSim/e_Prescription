using System.ComponentModel.DataAnnotations;

namespace e_Prescription.Models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Gender { get; set; }

        [RegularExpression("^(\\d{2})(0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01])(\\d{4})([01])(\\d)(\\d)$", ErrorMessage = "Invalid South African ID number format.")]
        public string IdNumber { get; set; }

       // public string Surgeon { get; set; }

        public bool IsActive { get; set; } = true;

    }
}
