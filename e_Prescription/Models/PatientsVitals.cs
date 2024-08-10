using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace e_Prescription.Models
{
    public class PatientsVitals
    {
        public PatientsVitals()
        {

        }

        [Key]
        public int PatientVitalId { get; set; }

        [ForeignKey("Id")]
        public int AdmissionId { get; set; }
        public virtual Admission Admission { get; set; }

        [Required(ErrorMessage = "*Required")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$|^\d{2,3}\/\d{2,3}$", ErrorMessage = "Invalid reading. Must be a double or in format 150/60.")]
        public string Reading { get; set; } // Can store either a double or a reading in the format 150/60

        [Required(ErrorMessage = "*Required")]
        public DateTime Time { get; set; }

        [ForeignKey(nameof(Vitals))]
        public int VitalId { get; set; }
        public virtual Vitals Vitals { get; set; }

        public string? Note { get; set; } = "Normal";
    }
}
